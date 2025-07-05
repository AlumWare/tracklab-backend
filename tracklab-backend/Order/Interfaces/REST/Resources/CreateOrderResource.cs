namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

using System.Collections.Generic;

public record CreateOrderResource(
    long CustomerId,
    long LogisticsId,
    string ShippingAddress,
    List<AddOrderItemResource> Items
); 