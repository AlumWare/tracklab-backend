namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

using System.Collections.Generic;

public record CreateOrderResource(
    long LogisticsId,
    string ShippingAddress,
    List<AddOrderItemResource> Items
); 