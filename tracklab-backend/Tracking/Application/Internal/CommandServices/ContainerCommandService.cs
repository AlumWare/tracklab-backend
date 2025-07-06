using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Application.Internal.OutboundServices.ACL;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using IResourceContextServiceTracking = Alumware.Tracklab.API.Tracking.Application.Internal.OutboundServices.ACL.IResourceContextService;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

public class ContainerCommandService : IContainerCommandService
{
    private readonly IContainerRepository _containerRepository;
    private readonly IOrderContextService _orderContextService;
    private readonly IResourceContextServiceTracking _resourceContextService;
    private readonly IQrCodeService _qrCodeService;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public ContainerCommandService(
        IContainerRepository containerRepository,
        IOrderContextService orderContextService,
        IResourceContextServiceTracking resourceContextService,
        IQrCodeService qrCodeService,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _containerRepository = containerRepository;
        _orderContextService = orderContextService;
        _resourceContextService = resourceContextService;
        _qrCodeService = qrCodeService;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Container> CreateAsync(CreateContainerCommand command)
    {
        var currentTenantId = _tenantContext.CurrentTenantId ?? throw new InvalidOperationException("No hay tenant context establecido");
        
        // Validar que la orden existe y tenemos acceso a ella
        if (!await _orderContextService.ValidateOrderExistsAsync(command.OrderId, currentTenantId))
        {
            throw new ArgumentException($"La orden con ID {command.OrderId} no existe o no tiene acceso a ella.");
        }
        
        // Validar que el almacén existe usando ACL de ResourceContext
        if (!await _resourceContextService.ValidateWarehouseExistsAsync(command.WarehouseId))
        {
            throw new ArgumentException($"El almacén con ID {command.WarehouseId} no existe o no está activo.");
        }
        
        // Obtener los productos de la orden para validaciones
        var orderItems = await _orderContextService.GetOrderItemsAsync(command.OrderId, currentTenantId);
        var orderItemDict = orderItems.ToDictionary(item => item.ProductId, item => item.Quantity);
        
        // Validar que todos los productos del contenedor están en la orden
        foreach (var shipItem in command.ShipItems)
        {
            if (!orderItemDict.ContainsKey(shipItem.ProductId))
            {
                throw new ArgumentException($"El producto con ID {shipItem.ProductId} no está incluido en la orden {command.OrderId}.");
            }
        }
        
        // Obtener cantidades ya asignadas a otros contenedores
        var assignedQuantities = await _orderContextService.GetAssignedQuantitiesAsync(command.OrderId);
        
        // Validar que las cantidades no excedan las de la orden
        foreach (var shipItem in command.ShipItems)
        {
            var currentAssigned = assignedQuantities.GetValueOrDefault(shipItem.ProductId, 0);
            var newTotal = currentAssigned + shipItem.Quantity;
            var orderQuantity = orderItemDict[shipItem.ProductId];
            
            if (newTotal > orderQuantity)
            {
                throw new ArgumentException(
                    $"La cantidad total para el producto {shipItem.ProductId} excedería la cantidad de la orden. " +
                    $"Orden: {orderQuantity}, Ya asignado: {currentAssigned}, Solicitado: {shipItem.Quantity}, " +
                    $"Total resultante: {newTotal}");
            }
        }
        
        // Crear el contenedor
        var container = new Container(command);
        
        // Persistir el contenedor primero para obtener el ID
        await _containerRepository.AddAsync(container);
        await _unitOfWork.CompleteAsync();
        
        // Generar QR Code automáticamente después de tener el ID del contenedor
        var trackingUrl = $"https://tracklab.com/tracking/{container.ContainerId}";
        var qrCode = await _qrCodeService.GenerateQrCodeAsync(container.ContainerId, trackingUrl);
        
        // Asignar el QR Code al contenedor
        container.AssignQrCode(qrCode);
        
        // Actualizar el contenedor con el QR Code
        _containerRepository.Update(container);
        await _unitOfWork.CompleteAsync();
        
        return container;
    }

    /// <summary>
    /// Completes a container when delivered at a CLIENT warehouse
    /// Also validates if the order is fully satisfied and marks it as completed
    /// </summary>
    public async Task<Container> CompleteContainerAsync(CompleteContainerCommand command)
    {
        var currentTenantId = _tenantContext.CurrentTenantId ?? 
            throw new InvalidOperationException("No hay tenant context establecido");

        // Get the container
        var container = await _containerRepository.GetByIdAsync(new GetContainerByIdQuery(command.ContainerId));
        if (container == null)
            throw new KeyNotFoundException($"Container {command.ContainerId} no encontrado.");

        if (container.IsCompleted)
            throw new InvalidOperationException($"El contenedor {command.ContainerId} ya está completado.");

        // Validate that the delivery warehouse exists and is CLIENT type
        var warehouse = await _resourceContextService.GetWarehouseAsync(command.DeliveryWarehouseId);
        if (warehouse == null)
            throw new ArgumentException($"El almacén con ID {command.DeliveryWarehouseId} no existe.");

        if (warehouse.Type != Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects.EWarehouseType.Client)
            throw new ArgumentException($"Solo se pueden completar contenedores en almacenes de tipo CLIENT.");

        // Complete the container
        container.CompleteContainer(command);
        _containerRepository.Update(container);

        // Check if the order is now fully satisfied
        var orderId = container.OrderId.Value;
        var isOrderComplete = await ValidateOrderCompletionAsync(orderId, currentTenantId);
        
        if (isOrderComplete)
        {
            // Mark the order as completed through ACL
            await _orderContextService.CompleteOrderAsync(orderId, currentTenantId);
        }

        await _unitOfWork.CompleteAsync();

        return container;
    }

    /// <summary>
    /// Validates if an order is fully satisfied by checking all containers
    /// </summary>
    private async Task<bool> ValidateOrderCompletionAsync(long orderId, long tenantId)
    {
        // Get all containers for this order
        var containers = await _containerRepository.GetByOrderIdAsync(orderId);
        
        // Get order items to validate against
        var orderItems = await _orderContextService.GetOrderItemsAsync(orderId, tenantId);
        var orderItemDict = orderItems.ToDictionary(item => item.ProductId, item => item.Quantity);
        
        // Calculate total delivered quantities from completed containers
        var deliveredQuantities = new Dictionary<long, int>();
        
        foreach (var container in containers.Where(c => c.IsCompleted))
        {
            var containerProducts = container.GetProductQuantities();
            foreach (var (productId, quantity) in containerProducts)
            {
                deliveredQuantities[productId] = deliveredQuantities.GetValueOrDefault(productId, 0) + quantity;
            }
        }
        
        // Check if all products are fully delivered
        foreach (var (productId, requiredQuantity) in orderItemDict)
        {
            var deliveredQuantity = deliveredQuantities.GetValueOrDefault(productId, 0);
            if (deliveredQuantity < requiredQuantity)
            {
                return false; // Order not yet complete
            }
        }
        
        return true; // All products fully delivered
    }

    public async Task<Container> UpdateCurrentNodeAsync(UpdateContainerNodeCommand command)
    {
        var container = await _containerRepository.GetByIdAsync(new GetContainerByIdQuery(command.ContainerId));
        if (container == null)
            throw new KeyNotFoundException($"Container {command.ContainerId} no encontrado.");
        container.UpdateCurrentNode(command);
        _containerRepository.Update(container);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return container;
    }
} 