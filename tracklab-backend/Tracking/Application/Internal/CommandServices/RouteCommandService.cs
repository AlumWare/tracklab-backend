using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using TrackLab.Shared.Domain.Repositories;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

public class RouteCommandService : IRouteCommandService
{
    private readonly IRouteRepository _routeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RouteCommandService(
        IRouteRepository routeRepository,
        IUnitOfWork unitOfWork)
    {
        _routeRepository = routeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RouteAggregate> CreateAsync(CreateRouteCommand command)
    {
        var route = new RouteAggregate(command);
        await _routeRepository.AddAsync(route);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return route;
    }

    public async Task<RouteAggregate> AddNodeAsync(AddNodeCommand command)
    {
        var route = await _routeRepository.GetByIdAsync(new GetRouteByIdQuery(command.RouteId));
        if (route == null)
            throw new KeyNotFoundException($"Ruta {command.RouteId} no encontrada.");
        route.AddNode(command);
        _routeRepository.Update(route);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return route;
    }

    public async Task<RouteAggregate> AddOrderAsync(AddOrderToRouteCommand command)
    {
        var route = await _routeRepository.GetByIdAsync(new GetRouteByIdQuery(command.RouteId));
        if (route == null)
            throw new KeyNotFoundException($"Ruta {command.RouteId} no encontrada.");
        route.AddOrder(command);
        _routeRepository.Update(route);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return route;
    }
} 