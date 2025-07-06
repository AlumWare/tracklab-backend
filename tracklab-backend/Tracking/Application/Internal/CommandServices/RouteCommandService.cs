using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Application.Internal.OutboundServices.ACL;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using IResourceContextServiceTracking = Alumware.Tracklab.API.Tracking.Application.Internal.OutboundServices.ACL.IResourceContextService;
using Microsoft.Extensions.Logging;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

/// <summary>
/// Service for managing route operations with business logic validation
/// </summary>
public class RouteCommandService : IRouteCommandService
{
    private readonly IRouteRepository _routeRepository;
    private readonly IOrderContextService _orderContextService;
    private readonly IResourceContextServiceTracking _resourceContextService;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RouteCommandService> _logger;

    public RouteCommandService(
        IRouteRepository routeRepository,
        IOrderContextService orderContextService,
        IResourceContextServiceTracking resourceContextService,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork,
        ILogger<RouteCommandService> logger)
    {
        _routeRepository = routeRepository;
        _orderContextService = orderContextService;
        _resourceContextService = resourceContextService;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new route with complete planning validation
    /// Validates vehicle and warehouse accessibility, and order assignments
    /// </summary>
    public async Task<RouteAggregate> CreateAsync(CreateRouteCommand command)
    {
        var currentTenantId = _tenantContext.CurrentTenantId ?? 
            throw new InvalidOperationException("No hay tenant context establecido");

        _logger.LogInformation("Creating route {RouteName} for tenant {TenantId}", 
            command.RouteName, currentTenantId);

        // Validate vehicle exists and is accessible
        if (!await _resourceContextService.ValidateVehicleExistsAsync(command.VehicleId))
        {
            throw new ArgumentException($"El vehículo con ID {command.VehicleId} no existe o no está disponible.");
        }

        // Validate all warehouses exist and are accessible
        foreach (var warehouseId in command.WarehouseIds)
        {
            if (!await _resourceContextService.ValidateWarehouseExistsAsync(warehouseId))
            {
                throw new ArgumentException($"El almacén con ID {warehouseId} no existe o no está activo.");
            }
        }

        // Validate all orders exist and are accessible by current tenant (logistics company)
        foreach (var orderId in command.OrderIds)
        {
            if (!await _orderContextService.ValidateOrderExistsAsync(orderId, currentTenantId))
            {
                throw new ArgumentException($"La orden con ID {orderId} no existe o no tiene acceso a ella.");
            }
        }

        // Create the route with validated information
        var route = new RouteAggregate(command);
        await _routeRepository.AddAsync(route);
        await _unitOfWork.CompleteAsync();

        // Now associate the validated orders to the route
        foreach (var orderId in command.OrderIds)
        {
            var order = await _orderContextService.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                route.AddOrder(order);
            }
        }
        
        _routeRepository.Update(route);
        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Route {RouteId} created successfully with {WarehouseCount} warehouses and {OrderCount} orders", 
            route.RouteId, command.WarehouseIds.Count(), command.OrderIds.Count());

        return route;
    }

    /// <summary>
    /// Adds a warehouse node to existing route with validation
    /// </summary>
    public async Task<RouteAggregate> AddNodeAsync(AddNodeCommand command)
    {
        var route = await _routeRepository.GetByIdAsync(new GetRouteByIdQuery(command.RouteId));
        if (route == null)
            throw new KeyNotFoundException($"Ruta {command.RouteId} no encontrada.");

        // Validate warehouse exists and is accessible
        if (!await _resourceContextService.ValidateWarehouseExistsAsync(command.WarehouseId))
        {
            throw new ArgumentException($"El almacén con ID {command.WarehouseId} no existe o no está activo.");
        }

        route.AddNode(command);
        _routeRepository.Update(route);
        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Warehouse {WarehouseId} added to route {RouteId}", 
            command.WarehouseId, command.RouteId);

        return route;
    }

    /// <summary>
    /// Adds an order to existing route with validation
    /// Ensures order is accessible by logistics company
    /// </summary>
    public async Task<RouteAggregate> AddOrderAsync(long routeId, Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order order)
    {
        var currentTenantId = _tenantContext.CurrentTenantId ?? 
            throw new InvalidOperationException("No hay tenant context establecido");

        var route = await _routeRepository.GetByIdAsync(new GetRouteByIdQuery(routeId));
        if (route == null)
            throw new KeyNotFoundException($"Ruta {routeId} no encontrada.");

        // Validate order accessibility
        if (!await _orderContextService.ValidateOrderExistsAsync(order.OrderId, currentTenantId))
        {
            throw new ArgumentException($"La orden con ID {order.OrderId} no existe o no tiene acceso a ella.");
        }

        // Validate order is not completed
        if (order.Status == Alumware.Tracklab.API.Order.Domain.Model.ValueObjects.OrderStatus.Delivered)
        {
            throw new ArgumentException($"No se puede asignar la orden {order.OrderId} porque ya está completada.");
        }

        route.AddOrder(order);
        _routeRepository.Update(route);
        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Order {OrderId} added to route {RouteId}", 
            order.OrderId, routeId);

        return route;
    }

    /// <summary>
    /// Gets available products for container creation based on route orders
    /// This is used by logistics companies to know what products can be packed
    /// </summary>
    public async Task<Dictionary<long, AvailableProductInfo>> GetAvailableProductsForRouteAsync(long routeId)
    {
        var currentTenantId = _tenantContext.CurrentTenantId ?? 
            throw new InvalidOperationException("No hay tenant context establecido");

        var route = await _routeRepository.GetByIdAsync(new GetRouteByIdQuery(routeId));
        if (route == null)
            throw new KeyNotFoundException($"Ruta {routeId} no encontrada.");

        var availableProducts = new Dictionary<long, AvailableProductInfo>();

        // For each order in the route, get available quantities
        foreach (var order in route.Orders)
        {
            var orderItems = await _orderContextService.GetOrderItemsAsync(order.OrderId, currentTenantId);
            var assignedQuantities = await _orderContextService.GetAssignedQuantitiesAsync(order.OrderId);

            foreach (var item in orderItems)
            {
                var assigned = assignedQuantities.GetValueOrDefault(item.ProductId, 0);
                var available = item.Quantity - assigned;

                if (available > 0)
                {
                    if (availableProducts.ContainsKey(item.ProductId))
                    {
                        availableProducts[item.ProductId] = availableProducts[item.ProductId] with 
                        { 
                            AvailableQuantity = availableProducts[item.ProductId].AvailableQuantity + available 
                        };
                    }
                    else
                    {
                        availableProducts[item.ProductId] = new AvailableProductInfo(
                            item.ProductId,
                            available,
                            order.OrderId
                        );
                    }
                }
            }
        }

        return availableProducts;
    }
}

/// <summary>
/// Information about available products for container creation
/// </summary>
public record AvailableProductInfo(
    long ProductId,
    int AvailableQuantity,
    long FromOrderId
); 