using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;

public static class RouteResourceFromEntityAssembler
{
    public static RouteResource ToResourceFromEntity(RouteAggregate route)
    {
        return new RouteResource(
            route.RouteId,
            route.VehicleId.Value,
            route.RouteItems.Select(ToRouteItemResourceFromEntity),
            route.Orders.Select(o => o.Value)
        );
    }

    public static IEnumerable<RouteResource> ToResourceFromEntities(IEnumerable<RouteAggregate> routes)
    {
        return routes.Select(ToResourceFromEntity);
    }

    private static RouteItemResource ToRouteItemResourceFromEntity(Domain.Model.Entities.RouteItem item)
    {
        return new RouteItemResource(
            item.Id,
            item.WarehouseId.Value,
            item.IsCompleted,
            item.CompletedAt
        );
    }
} 