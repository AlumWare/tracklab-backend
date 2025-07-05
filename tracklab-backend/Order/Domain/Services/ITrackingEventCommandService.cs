using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Order.Domain.Services;

public interface ITrackingEventCommandService
{
    Task<TrackingEvent> RegisterTrackingEventAsync(RegisterTrackingEventCommand command);
    Task<List<TrackingEvent>> GetTrackingEventsAsync(long orderId);
} 