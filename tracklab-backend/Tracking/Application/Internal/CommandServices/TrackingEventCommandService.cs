using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using TrackLab.Shared.Domain.Repositories;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

public class TrackingEventCommandService : ITrackingEventCommandService
{
    private readonly ITrackingEventRepository _trackingEventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TrackingEventCommandService(
        ITrackingEventRepository trackingEventRepository,
        IUnitOfWork unitOfWork)
    {
        _trackingEventRepository = trackingEventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TrackingEvent> CreateAsync(CreateTrackingEventCommand command)
    {
        var trackingEvent = new TrackingEvent(command);
        await _trackingEventRepository.AddAsync(trackingEvent);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return trackingEvent;
    }
} 