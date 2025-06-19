using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.QueryServices;

public class TrackingEventQueryService : ITrackingEventQueryService
{
    private readonly ITrackingEventRepository _trackingEventRepository;

    public TrackingEventQueryService(ITrackingEventRepository trackingEventRepository)
    {
        _trackingEventRepository = trackingEventRepository;
    }

    public async Task<IEnumerable<TrackingEvent>> Handle(GetAllTrackingEventsQuery query)
    {
        return await _trackingEventRepository.GetAllAsync(query);
    }

    public async Task<TrackingEvent?> Handle(GetTrackingEventByIdQuery query)
    {
        return await _trackingEventRepository.GetByIdAsync(query);
    }
} 