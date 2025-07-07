using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

public record OrderResource(
    long OrderId,
    long CustomerId,
    long LogisticsId,
    string ShippingAddress,
    DateTime OrderDate,
    string Status,
    decimal TotalPrice,
    IEnumerable<OrderItemResource> OrderItems
);

public record OrderItemResource(
    long Id,
    long ProductId,
    int Quantity,
    decimal PriceAmount,
    string PriceCurrency
); 