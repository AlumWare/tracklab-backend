using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Domain.Services;

public interface IRouteQueryService
{
    Task<IEnumerable<RouteAggregate>> Handle(GetAllRoutesQuery query);
    Task<RouteAggregate?> Handle(GetRouteByIdQuery query);
} 