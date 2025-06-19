using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

public class TrackingEventCommandService : ITrackingEventCommandService
{
    private readonly ITrackingEventRepository _trackingEventRepository;

    public TrackingEventCommandService(ITrackingEventRepository trackingEventRepository)
    {
        _trackingEventRepository = trackingEventRepository;
    }

    public async Task<TrackingEvent> CreateAsync(CreateTrackingEventCommand command)
    {
        var trackingEvent = new TrackingEvent(command);
        await _trackingEventRepository.AddAsync(trackingEvent);
        return trackingEvent;
    }
} 