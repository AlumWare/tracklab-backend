namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;
using System.Collections.Generic;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

public record CreateOrderCommand(
    long CustomerId,
    long LogisticsId,
    string ShippingAddress,
    List<AddOrderItemResource> Items
); 