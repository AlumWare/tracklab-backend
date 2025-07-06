using Alumware.Tracklab.API.Order.Domain.Model.Entities;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;
using OrderAggregate = Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class OrderResourceFromEntityAssembler
{
    public static OrderResource ToResourceFromEntity(OrderAggregate order)
    {
        return new OrderResource(
            order.OrderId,
            order.TenantId,
            order.LogisticsId,
            order.ShippingAddress,
            order.OrderDate,
            order.Status.ToString(),
            order.GetTotalOrderPrice(),
            (order.OrderItems ?? new List<OrderItem>()).Select(orderItem => new OrderItemResource(
                orderItem.Id,
                orderItem.ProductId,
                orderItem.Quantity,
                orderItem.Price.Amount,
                orderItem.Price.Currency
            )).ToList()
        );
    }

    public static IEnumerable<OrderResource> ToResourceFromEntities(IEnumerable<OrderAggregate> orders)
    {
        return orders.Select(ToResourceFromEntity);
    }

    private static OrderItemResource ToOrderItemResourceFromEntity(OrderItem orderItem)
    {
        return new OrderItemResource(
            orderItem.Id,
            orderItem.ProductId,
            orderItem.Quantity,
            orderItem.Price.Amount,
            orderItem.Price.Currency
        );
    }
} 