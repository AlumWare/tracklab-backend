namespace Alumware.Tracklab.API.Tracking.Domain.Model.Commands;

/// <summary>
/// Command for creating a complete route with planning information
/// </summary>
public record CreateRouteCommand(
    long VehicleId,
    string RouteName,
    IEnumerable<long> WarehouseIds,
    IEnumerable<long> OrderIds,
    DateTime? PlannedStartDate = null,
    string? Description = null
); 