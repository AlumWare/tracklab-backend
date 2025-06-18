using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Domain.Repositories;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Application.Internal.CommandServices;

public class OrderCommandService : IOrderCommandService
{
    private readonly IOrderRepository _orderRepository;

    public OrderCommandService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderAggregate> CreateAsync(CreateOrderCommand command)
    {
        var order = new OrderAggregate(command);
        await _orderRepository.AddAsync(order);
        return order;
    }

    public async Task<OrderAggregate> AddOrderItemAsync(AddOrderItemCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {command.OrderId} no encontrada.");

        order.AddOrderItem(command);
        _orderRepository.Update(order);
        return order;
    }

    public async Task<OrderAggregate> UpdateStatusAsync(UpdateOrderStatusCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {command.OrderId} no encontrada.");

        order.UpdateStatus(command);
        _orderRepository.Update(order);
        return order;
    }

    public async Task DeleteAsync(DeleteOrderCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null)
            throw new KeyNotFoundException($"Orden {command.OrderId} no encontrada.");

        _orderRepository.Remove(order);
    }
} 