namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;

public record CreateOrderCommand(
    long CustomerId,
    long LogisticsId,
    string ShippingAddress
); 