using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Domain.Events;
using Alumware.Tracklab.API.Order.Domain.Events;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Application.Internal.CommandServices;

public class OrderCommandService : IOrderCommandService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly IDomainEventDispatcher _eventDispatcher;

    public OrderCommandService(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext,
        IDomainEventDispatcher eventDispatcher)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<OrderAggregate> CreateAsync(CreateOrderCommand command)
    {
        var order = new OrderAggregate(command);
        
        // Establecer el tenant_id desde el contexto actual
        if (_tenantContext.HasTenant)
        {
            order.SetTenantId(_tenantContext.CurrentTenantId!.Value);
        }
        
        await _orderRepository.AddAsync(order);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        
        
        return order;
    }

    public async Task<OrderAggregate> AddOrderItemAsync(AddOrderItemCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {command.OrderId} no encontrada.");

        order.AddOrderItem(command);
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return order;
    }

    public async Task<OrderAggregate> UpdateStatusAsync(UpdateOrderStatusCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {command.OrderId} no encontrada.");

        order.UpdateStatus(command);
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return order;
    }

    public async Task DeleteAsync(DeleteOrderCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {command.OrderId} no encontrada.");

        _orderRepository.Remove(order);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task<OrderAggregate> AssignLogisticsAndVehicleAsync(AssignLogisticsAndVehicleCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {command.OrderId} no encontrada.");
        if (order.Status != Alumware.Tracklab.API.Order.Domain.Model.ValueObjects.OrderStatus.Pending)
            throw new InvalidOperationException("Solo se puede asignar logística y vehículo a órdenes pendientes.");
        // Aquí deberías obtener el vehículo y validar tonelaje, estado, etc. (pseudo-código):
        // var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        // if (vehicle == null || vehicle.Status != Available || vehicle.Tonnage < order.GetTotalWeight())
        //     throw new InvalidOperationException("Vehículo no disponible o tonelaje insuficiente.");
        order.AssignLogisticsAndVehicle(command.LogisticsId, command.VehicleId);
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();
        return order;
    }

    public async Task<OrderAggregate> SetRouteAsync(SetRouteCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {command.OrderId} no encontrada.");
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
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {command.OrderId} no encontrada.");
        
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
} 