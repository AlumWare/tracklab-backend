namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

public record CreateOrderResource(
    long CustomerId,
    long LogisticsId,
    string ShippingAddress
); 