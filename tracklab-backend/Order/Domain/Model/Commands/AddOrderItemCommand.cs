namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;

public record AddOrderItemCommand(
    long OrderId,
    long ProductId,
    int Quantity,
    decimal PriceAmount,
    string PriceCurrency
); 