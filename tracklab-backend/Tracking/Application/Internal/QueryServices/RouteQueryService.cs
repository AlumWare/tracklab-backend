using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.QueryServices;

public class RouteQueryService : IRouteQueryService
{
    private readonly IRouteRepository _routeRepository;

    public RouteQueryService(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    public async Task<IEnumerable<RouteAggregate>> Handle(GetAllRoutesQuery query)
    {
        return await _routeRepository.GetAllAsync(query);
    }

    public async Task<RouteAggregate?> Handle(GetRouteByIdQuery query)
    {
        return await _routeRepository.GetByIdAsync(query);
    }
} 