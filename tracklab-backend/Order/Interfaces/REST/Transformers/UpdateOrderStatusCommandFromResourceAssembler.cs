using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class UpdateOrderStatusCommandFromResourceAssembler
{
    public static UpdateOrderStatusCommand ToCommandFromResource(long orderId, UpdateOrderStatusResource resource)
    {
        if (!Enum.TryParse<OrderStatus>(resource.Status, out var status))
        {
            throw new ArgumentException($"Estado de orden inválido: {resource.Status}");
        }

        return new UpdateOrderStatusCommand(orderId, status);
    }
} 