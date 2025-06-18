namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

public record AddOrderItemResource(
    long ProductId,
    int Quantity,
    decimal PriceAmount,
    string PriceCurrency
); 