using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using TrackLab.Shared.Domain.Repositories;

namespace Alumware.Tracklab.API.Tracking.Domain.Repositories;

public interface ITrackingEventRepository : IBaseRepository<TrackingEvent>
{
    Task<IEnumerable<TrackingEvent>> GetAllAsync(GetAllTrackingEventsQuery query);
    Task<TrackingEvent?> GetByIdAsync(GetTrackingEventByIdQuery query);
    Task<IEnumerable<TrackingEvent>> GetByContainerIdAsync(long containerId);
} 