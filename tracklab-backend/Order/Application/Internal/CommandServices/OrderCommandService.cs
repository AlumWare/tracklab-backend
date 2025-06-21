using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Application.Internal.CommandServices;

public class OrderCommandService : IOrderCommandService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public OrderCommandService(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<OrderAggregate> CreateAsync(CreateOrderCommand command)
    {
        var order = new OrderAggregate(command);
        
        // Establecer el tenant_id desde el contexto actual
        if (_tenantContext.HasTenant)
        {
            order.SetTenantId(new TrackLab.Shared.Domain.ValueObjects.TenantId(_tenantContext.CurrentTenantId!.Value));
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
} 