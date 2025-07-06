using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Domain.Services;

public interface IOrderCommandService
{
    Task<OrderAggregate> CreateWithProductInfoAsync(CreateOrderWithProductInfoCommand command);
    Task<OrderAggregate> AddOrderItemAsync(AddOrderItemCommand command);
    Task<OrderAggregate> UpdateStatusAsync(UpdateOrderStatusCommand command);
    Task DeleteAsync(DeleteOrderCommand command);
    Task<OrderAggregate> AssignVehicleAsync(AssignVehicleCommand command);
    Task<OrderAggregate> SetRouteAsync(SetRouteCommand command);
} 