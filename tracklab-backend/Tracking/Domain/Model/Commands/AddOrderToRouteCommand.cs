namespace Alumware.Tracklab.API.Tracking.Domain.Model.Commands;

/// <summary>
/// Command for adding an order to an existing route
/// </summary>
public record AddOrderToRouteCommand(
    long RouteId,
    long OrderId
); 