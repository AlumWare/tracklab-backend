using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Domain.Events;
using Alumware.Tracklab.API.Order.Domain.Events;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;
using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Domain.Repositories;
using TrackLab.IAM.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Order.Application.Internal.OutboundServices.ACL;
using IResourceContextService = Alumware.Tracklab.API.Order.Application.Internal.OutboundServices.ACL.IResourceContextService;

namespace Alumware.Tracklab.API.Order.Application.Internal.CommandServices;

public class OrderCommandService : IOrderCommandService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITrackingContextService _trackingContextService;
    private readonly IResourceContextService _resourceContextService;
    private readonly IDomainEventDispatcher _eventDispatcher;

    public OrderCommandService(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext,
        ITenantRepository tenantRepository,
        ITrackingContextService trackingContextService,
        IResourceContextService resourceContextService,
        IDomainEventDispatcher eventDispatcher)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _tenantRepository = tenantRepository;
        _trackingContextService = trackingContextService;
        _resourceContextService = resourceContextService;
        _eventDispatcher = eventDispatcher;
    }

    /// <summary>
    /// Validates that the current tenant has access to the order (either as customer or logistics company)
    /// </summary>
    private async Task<OrderAggregate> GetOrderWithAccessValidationAsync(long orderId)
    {
        var order = await _orderRepository.FindByIdAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {orderId} no encontrada.");

        // Validar acceso del tenant actual
        if (_tenantContext.HasTenant)
        {
            var currentTenantId = _tenantContext.CurrentTenantId!.Value;
            var hasAccess = order.TenantId == currentTenantId || order.LogisticsId == currentTenantId;
            
            if (!hasAccess)
                throw new UnauthorizedAccessException("No tienes acceso a esta orden.");
        }

        return order;
    }

    private async Task<AddOrderItemWithPriceCommand> ValidateAndEnrichOrderItemAsync(AddOrderItemWithPriceCommand item)
    {
        // Validar que el producto existe usando ACL
        if (!await _resourceContextService.ValidateProductExistsAsync(item.ProductId))
            throw new ArgumentException($"Producto con ID {item.ProductId} no encontrado o sin stock disponible.");
        
        // Obtener información del producto usando ACL
        var productInfo = await _resourceContextService.GetProductInfoAsync(item.ProductId);
        if (productInfo == null)
            throw new ArgumentException($"No se pudo obtener información del producto con ID {item.ProductId}.");
        
        if (productInfo.Stock < item.Quantity)
            throw new ArgumentException($"Stock insuficiente para el producto {productInfo.Name}. Disponible: {productInfo.Stock}, Solicitado: {item.Quantity}");
        
        // Retornar el comando con la información correcta del producto
        return new AddOrderItemWithPriceCommand(
            item.ProductId,
            item.Quantity,
            productInfo.Price,
            productInfo.Currency
        );
    }

    public async Task<OrderAggregate> CreateWithProductInfoAsync(CreateOrderWithProductInfoCommand command)
    {
        // Validar que el logisticsId corresponde a un tenant de tipo LOGISTIC
        var logisticsTenant = await _tenantRepository.FindByIdAsync(command.LogisticsId);
        if (logisticsTenant == null || !logisticsTenant.IsLogistic())
            throw new InvalidOperationException("El LogisticsId no corresponde a una empresa logística válida.");

        // Validar y enriquecer cada item con información del producto
        var validatedItems = new List<AddOrderItemWithPriceCommand>();
        foreach (var item in command.Items)
        {
            var validatedItem = await ValidateAndEnrichOrderItemAsync(item);
            validatedItems.Add(validatedItem);
        }

        // Crear el comando con los items validados
        var enrichedCommand = new CreateOrderWithProductInfoCommand(
            command.CustomerId,
            command.LogisticsId,
            command.ShippingAddress,
            validatedItems
        );

        var order = new OrderAggregate(enrichedCommand);
        
        // Establecer el tenant_id desde el contexto actual
        if (_tenantContext.HasTenant)
        {
            order.SetTenantId(_tenantContext.CurrentTenantId!.Value);
        }
        
        await _orderRepository.AddAsync(order);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        
        // Calculate totals for the event
        var totalAmount = validatedItems.Sum(item => item.PriceAmount * item.Quantity);
        var totalItems = validatedItems.Sum(item => item.Quantity);
        var currency = validatedItems.First().PriceCurrency;
        
        // Publish OrderCreatedEvent
        var orderCreatedEvent = new OrderCreatedEvent(
            order.OrderId,
            command.CustomerId,
            command.LogisticsId,
            command.ShippingAddress,
            totalAmount,
            currency,
            totalItems
        );
        
        await _eventDispatcher.PublishAsync(orderCreatedEvent);
        
        return order;
    }

    public async Task<OrderAggregate> AddOrderItemAsync(AddOrderItemCommand command)
    {
        var order = await GetOrderWithAccessValidationAsync(command.OrderId);

        // Validar el producto y obtener su información
        if (!await _resourceContextService.ValidateProductExistsAsync(command.ProductId))
            throw new ArgumentException($"Producto con ID {command.ProductId} no encontrado o sin stock disponible.");
        
        var productInfo = await _resourceContextService.GetProductInfoAsync(command.ProductId);
        if (productInfo == null)
            throw new ArgumentException($"No se pudo obtener información del producto con ID {command.ProductId}.");
        
        if (productInfo.Stock < command.Quantity)
            throw new ArgumentException($"Stock insuficiente para el producto {productInfo.Name}. Disponible: {productInfo.Stock}, Solicitado: {command.Quantity}");

        // Crear el comando con la información correcta del producto
        var enrichedCommand = new AddOrderItemCommand(
            command.OrderId,
            command.ProductId,
            command.Quantity,
            productInfo.Price,
            productInfo.Currency
        );

        order.AddOrderItem(enrichedCommand);
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return order;
    }

    public async Task<OrderAggregate> UpdateStatusAsync(UpdateOrderStatusCommand command)
    {
        var order = await GetOrderWithAccessValidationAsync(command.OrderId);

        // Validar si el cambio de estado es permitido basado en el estado de tracking
        var newStatus = command.NewStatus.ToString();
        var canChangeStatus = await _trackingContextService.CanOrderChangeStatusAsync(command.OrderId, newStatus);
        
        if (!canChangeStatus)
            throw new InvalidOperationException($"No se puede cambiar el estado de la orden a '{newStatus}' debido a restricciones de tracking.");

        // Store previous status for the event
        var previousStatus = order.Status;
        
        order.UpdateStatus(command);
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        
        // Publish OrderStatusChangedEvent
        var statusChangedEvent = new OrderStatusChangedEvent(
            order.OrderId,
            previousStatus,
            command.NewStatus,
            order.TenantId,
            order.LogisticsId,
            null // No reason provided in this implementation
        );
        
        await _eventDispatcher.PublishAsync(statusChangedEvent);
        
        return order;
    }

    public async Task DeleteAsync(DeleteOrderCommand command)
    {
        var order = await GetOrderWithAccessValidationAsync(command.OrderId);

        // Validar si la orden puede ser eliminada basado en el estado de tracking
        var canDelete = await _trackingContextService.CanOrderBeDeletedAsync(command.OrderId);
        
        if (!canDelete)
            throw new InvalidOperationException("No se puede eliminar la orden porque tiene contenedores activos o en tránsito.");

        // Store order info for the event
        var previousStatus = order.Status;
        
        _orderRepository.Remove(order);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        
        // Publish OrderStatusChangedEvent for cancellation
        var statusChangedEvent = new OrderStatusChangedEvent(
            order.OrderId,
            previousStatus,
            Alumware.Tracklab.API.Order.Domain.Model.ValueObjects.OrderStatus.Cancelled,
            order.TenantId,
            order.LogisticsId,
            "Orden eliminada por el usuario"
        );
        
        await _eventDispatcher.PublishAsync(statusChangedEvent);
    }

    public async Task<OrderAggregate> SetRouteAsync(SetRouteCommand command)
    {
        var order = await GetOrderWithAccessValidationAsync(command.OrderId);
        
        if (order.Status != Alumware.Tracklab.API.Order.Domain.Model.ValueObjects.OrderStatus.InProcess)
            throw new InvalidOperationException("Solo se puede definir/modificar la ruta de órdenes en proceso.");
        if (command.Warehouses == null || command.Warehouses.Count == 0)
            throw new InvalidOperationException("La ruta debe tener al menos un almacén.");
        order.SetRoute(command.VehicleId, command.Warehouses);
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();
        return order;
    }

    public async Task<OrderAggregate> AssignVehicleAsync(AssignVehicleCommand command)
    {
        var order = await GetOrderWithAccessValidationAsync(command.OrderId);
        
        if (order.Status != Alumware.Tracklab.API.Order.Domain.Model.ValueObjects.OrderStatus.Pending)
            throw new InvalidOperationException("Solo se puede asignar vehículo a órdenes pendientes.");
        
        // Aquí deberías validar que el vehículo existe y está disponible
        // var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        // if (vehicle == null || vehicle.Status != Available)
        //     throw new InvalidOperationException("Vehículo no disponible.");
        
        order.AssignVehicle(command.VehicleId);
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();
        return order;
    }

    /// <summary>
    /// Completes an order when all containers are delivered
    /// </summary>
    public async Task<OrderAggregate> CompleteOrderAsync(CompleteOrderCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Order {command.OrderId} not found");

        // Update order status to Delivered using the correct method
        var updateStatusCommand = new UpdateOrderStatusCommand(
            command.OrderId,
            Alumware.Tracklab.API.Order.Domain.Model.ValueObjects.OrderStatus.Delivered
        );
        
        order.UpdateStatus(updateStatusCommand);
        
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();

        return order;
    }
} 