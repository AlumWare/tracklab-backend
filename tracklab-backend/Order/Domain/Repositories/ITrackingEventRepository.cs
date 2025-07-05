using Alumware.Tracklab.API.Order.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Order.Domain.Repositories;

public interface ITrackingEventRepository
{
    Task AddAsync(TrackingEvent trackingEvent);
    Task<List<TrackingEvent>> GetByOrderIdAsync(long orderId);
    Task<TrackingEvent?> GetLastByOrderIdAsync(long orderId);
} 