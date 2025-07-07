using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;

/// <summary>
/// Assembler to transform Route entities to RouteResource DTOs
/// </summary>
public static class RouteResourceFromEntityAssembler
{
    /// <summary>
    /// Transforms a Route entity to RouteResource with complete planning information
    /// </summary>
    public static RouteResource ToResourceFromEntity(RouteAggregate route)
    {
        return new RouteResource(
            route.RouteId,
            route.VehicleId.Value,
            route.RouteName,
            route.PlannedStartDate,
            route.Description,
            route.CreatedDate?.DateTime ?? DateTime.UtcNow,
            route.IsActive,
            route.RouteItems.Select(ToRouteItemResourceFromEntity),
            route.Orders.Select(ToRouteOrderSummaryFromEntity)
        );
    }

    /// <summary>
    /// Transforms multiple Route entities to RouteResource collection
    /// </summary>
    public static IEnumerable<RouteResource> ToResourceFromEntities(IEnumerable<RouteAggregate> routes)
    {
        return routes.Select(ToResourceFromEntity);
    }

    /// <summary>
    /// Transforms RouteItem entity to RouteItemResource
    /// </summary>
    private static RouteItemResource ToRouteItemResourceFromEntity(Domain.Model.Entities.RouteItem item)
    {
        return new RouteItemResource(
            item.Id,
            item.WarehouseId.Value,
            item.IsCompleted,
            item.CompletedAt
        );
    }

    /// <summary>
    /// Transforms Order entity to RouteOrderSummary for route planning
    /// </summary>
    private static RouteOrderSummary ToRouteOrderSummaryFromEntity(Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order order)
    {
        return new RouteOrderSummary(
            order.OrderId,
            order.TenantId,
            order.LogisticsId,
            order.Status.ToString(),
            order.OrderItems?.Sum(item => item.Quantity) ?? 0,
            order.ShippingAddress
        );
    }
} 