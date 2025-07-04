namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

public record TrackingEventResource(
    long EventId,
    long ContainerId,
    long WarehouseId,
    string Type,
    DateTime EventTime
); 