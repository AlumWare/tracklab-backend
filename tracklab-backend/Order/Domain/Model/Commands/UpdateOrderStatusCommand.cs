using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;

public record UpdateOrderStatusCommand(
    long OrderId,
    OrderStatus NewStatus
); 