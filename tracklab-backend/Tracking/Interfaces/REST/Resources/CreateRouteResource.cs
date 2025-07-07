namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

/// <summary>
/// Resource for creating a new route with complete planning information
/// </summary>
public record CreateRouteResource(
    long VehicleId,
    string RouteName,
    IEnumerable<long> WarehouseIds,
    IEnumerable<long> OrderIds,
    DateTime? PlannedStartDate = null,
    string? Description = null
);

/// <summary>
/// Resource for adding a warehouse node to an existing route
/// </summary>
public record AddNodeResource(
    long WarehouseId
);

/// <summary>
/// Resource for adding an order to an existing route
/// </summary>
public record AddOrderToRouteResource(
    long OrderId
); 