using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Commands;

public record CreateTrackingEventCommand(
    long ContainerId,
    long WarehouseId,
    EventType Type,
    DateTime EventTime
); 