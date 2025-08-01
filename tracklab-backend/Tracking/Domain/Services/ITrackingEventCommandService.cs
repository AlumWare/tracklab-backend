using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Tracking.Domain.Services;

public interface ITrackingEventCommandService
{
    Task<TrackingEvent> CreateAsync(CreateTrackingEventCommand command);
} 