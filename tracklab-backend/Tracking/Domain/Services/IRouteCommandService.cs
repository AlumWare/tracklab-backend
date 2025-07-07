using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;
using RouteAggregate = Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates.Route;

namespace Alumware.Tracklab.API.Tracking.Domain.Services;

/// <summary>
/// Service interface for route command operations
/// </summary>
public interface IRouteCommandService
{
    /// <summary>
    /// Creates a new route with planning validation
    /// </summary>
    Task<RouteAggregate> CreateAsync(CreateRouteCommand command);
    
    /// <summary>
    /// Adds a warehouse node to an existing route
    /// </summary>
    Task<RouteAggregate> AddNodeAsync(AddNodeCommand command);
    
    /// <summary>
    /// Adds an order to an existing route
    /// </summary>
    Task<RouteAggregate> AddOrderAsync(long routeId, Alumware.Tracklab.API.Order.Domain.Model.Aggregates.Order order);
    
    /// <summary>
    /// Gets available products for container creation based on route orders
    /// </summary>
    Task<Dictionary<long, AvailableProductInfo>> GetAvailableProductsForRouteAsync(long routeId);
} 