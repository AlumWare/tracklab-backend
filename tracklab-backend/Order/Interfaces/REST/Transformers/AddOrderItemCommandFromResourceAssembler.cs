using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class AddOrderItemCommandFromResourceAssembler
{
    public static AddOrderItemCommand ToCommandFromResource(
        long orderId, 
        AddOrderItemResource resource)
    {
        return new AddOrderItemCommand(
            orderId,
            resource.ProductId,
            resource.Quantity,
            0, // Price will be validated and set by the command service
            string.Empty // Currency will be validated and set by the command service
        );
    }
} 