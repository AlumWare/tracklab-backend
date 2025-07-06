using System.Linq;
using System.Collections.Generic;
using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class CreateOrderCommandFromResourceAssembler
{
    public static CreateOrderWithProductInfoCommand ToCommandFromResource(CreateOrderResource resource)
    {
        var items = resource.Items.Select(item => new AddOrderItemWithPriceCommand(
            item.ProductId,
            item.Quantity,
            0, // Price will be validated and set by the command service
            string.Empty // Currency will be validated and set by the command service
        )).ToList();
        
        return new CreateOrderWithProductInfoCommand(
            resource.CustomerId,
            resource.LogisticsId,
            resource.ShippingAddress,
            items
        );
    }
} 