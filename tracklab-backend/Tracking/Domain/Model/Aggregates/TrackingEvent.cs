using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

public partial class TrackingEvent
{
    public long EventId { get; private set; }
    public long ContainerId { get; private set; }
    public WarehouseId WarehouseId { get; private set; } = null!;
    public EventType Type { get; private set; }
    public DateTime EventTime { get; private set; }

    public TrackingEvent() { }

    public TrackingEvent(CreateTrackingEventCommand command)
    {
        ContainerId = command.ContainerId;
        WarehouseId = new WarehouseId(command.WarehouseId);
        Type = command.Type;
        EventTime = command.EventTime;
    }
} 