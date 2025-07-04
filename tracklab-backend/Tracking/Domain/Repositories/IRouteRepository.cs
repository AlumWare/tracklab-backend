using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using TrackLab.Shared.Domain.Repositories;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Domain.Repositories;

public interface IRouteRepository : IBaseRepository<RouteAggregate>
{
    Task<IEnumerable<RouteAggregate>> GetAllAsync(GetAllRoutesQuery query);
    Task<RouteAggregate?> GetByIdAsync(GetRouteByIdQuery query);
} 