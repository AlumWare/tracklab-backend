namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

/// <summary>
/// Resource representing a complete route with planning and execution details
/// </summary>
public record RouteResource(
    long RouteId,
    long VehicleId,
    string RouteName,
    DateTime? PlannedStartDate,
    string? Description,
    DateTime CreatedAt,
    bool IsActive,
    IEnumerable<RouteItemResource> RouteItems,
    IEnumerable<RouteOrderSummary> Orders
);

/// <summary>
/// Resource representing a warehouse node in the route
/// </summary>
public record RouteItemResource(
    long Id,
    long WarehouseId,
    bool IsCompleted,
    DateTime? CompletedAt
);

/// <summary>
/// Resource representing order summary for route planning
/// </summary>
public record RouteOrderSummary(
    long OrderId,
    long CustomerId,
    long LogisticsId,
    string Status,
    int TotalItems,
    string ShippingAddress
); 