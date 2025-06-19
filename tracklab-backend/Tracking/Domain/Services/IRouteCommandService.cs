using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Domain.Services;

public interface IRouteCommandService
{
    Task<RouteAggregate> CreateAsync(CreateRouteCommand command);
    Task<RouteAggregate> AddNodeAsync(AddNodeCommand command);
    Task<RouteAggregate> AddOrderAsync(AddOrderToRouteCommand command);
} 