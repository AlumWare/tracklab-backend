using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;

public static class TrackingEventResourceFromEntityAssembler
{
    public static TrackingEventResource ToResourceFromEntity(TrackingEvent trackingEvent)
    {
        return new TrackingEventResource(
            trackingEvent.EventId,
            trackingEvent.ContainerId,
            trackingEvent.WarehouseId.Value,
            trackingEvent.Type.ToString(),
            trackingEvent.EventTime
        );
    }

    public static IEnumerable<TrackingEventResource> ToResourceFromEntities(IEnumerable<TrackingEvent> events)
    {
        return events.Select(ToResourceFromEntity);
    }
} 