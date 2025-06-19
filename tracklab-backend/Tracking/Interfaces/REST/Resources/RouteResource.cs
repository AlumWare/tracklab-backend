namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

public record RouteResource(
    long RouteId,
    long VehicleId,
    IEnumerable<RouteItemResource> RouteItems,
    IEnumerable<long> Orders
);

public record RouteItemResource(
    long Id,
    long WarehouseId,
    bool IsCompleted,
    DateTime? CompletedAt
); 