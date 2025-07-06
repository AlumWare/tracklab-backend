using System.Linq;
using System.Collections.Generic;
using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class CreateOrderCommandFromResourceAssembler
{
    public static CreateOrderCommand ToCommandFromResource(CreateOrderResource resource)
    {
        var items = resource.Items.Select(item => new AddOrderItemResource(
            item.ProductId,
            item.Quantity,
            item.PriceAmount,
            item.PriceCurrency
        )).ToList();
        return new CreateOrderCommand(
            resource.CustomerId,
            resource.LogisticsId,
            resource.ShippingAddress,
            items
        );
    }
} 