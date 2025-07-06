namespace Alumware.Tracklab.API.Tracking.Domain.Model.Commands;

/// <summary>
/// Command for adding a warehouse node to an existing route
/// </summary>
public record AddNodeCommand(
    long RouteId,
    long WarehouseId
); 