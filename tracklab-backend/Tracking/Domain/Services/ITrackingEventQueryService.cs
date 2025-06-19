using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Tracking.Domain.Services;

public interface ITrackingEventQueryService
{
    Task<IEnumerable<TrackingEvent>> Handle(GetAllTrackingEventsQuery query);
    Task<TrackingEvent?> Handle(GetTrackingEventByIdQuery query);
} 