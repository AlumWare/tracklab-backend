namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;
using System.Collections.Generic;

public record CreateOrderWithProductInfoCommand(
    long LogisticsId,
    string ShippingAddress,
    List<AddOrderItemWithPriceCommand> Items
);

public record AddOrderItemWithPriceCommand(
    long ProductId,
    int Quantity,
    decimal PriceAmount,
    string PriceCurrency
); 