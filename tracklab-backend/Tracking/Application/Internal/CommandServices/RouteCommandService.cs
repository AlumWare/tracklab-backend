using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

public class RouteCommandService : IRouteCommandService
{
    private readonly IRouteRepository _routeRepository;

    public RouteCommandService(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    public async Task<RouteAggregate> CreateAsync(CreateRouteCommand command)
    {
        var route = new RouteAggregate(command);
        await _routeRepository.AddAsync(route);
        return route;
    }

    public async Task<RouteAggregate> AddNodeAsync(AddNodeCommand command)
    {
        var route = await _routeRepository.GetByIdAsync(new GetRouteByIdQuery(command.RouteId));
        if (route == null)
            throw new KeyNotFoundException($"Ruta {command.RouteId} no encontrada.");
        route.AddNode(command);
        _routeRepository.Update(route);
        return route;
    }

    public async Task<RouteAggregate> AddOrderAsync(AddOrderToRouteCommand command)
    {
        var route = await _routeRepository.GetByIdAsync(new GetRouteByIdQuery(command.RouteId));
        if (route == null)
            throw new KeyNotFoundException($"Ruta {command.RouteId} no encontrada.");
        route.AddOrder(command);
        _routeRepository.Update(route);
        return route;
    }
} 