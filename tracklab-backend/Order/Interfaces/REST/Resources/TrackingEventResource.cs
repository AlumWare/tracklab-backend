namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

public record TrackingEventResource(
    long OrderId,
    string EventType,
    long WarehouseId,
    DateTime EventTime
); 