using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class AddOrderItemCommandFromResourceAssembler
{
    public static AddOrderItemCommand ToCommandFromResource(long orderId, AddOrderItemResource resource)
    {
        return new AddOrderItemCommand(
            orderId,
            resource.ProductId,
            resource.Quantity,
            resource.PriceAmount,
            resource.PriceCurrency
        );
    }
} 