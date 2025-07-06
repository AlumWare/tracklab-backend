using Alumware.Tracklab.API.Tracking.Interfaces.ACL;

namespace Alumware.Tracklab.API.Order.Application.Internal.OutboundServices.ACL;

public class TrackingContextService : ITrackingContextService
{
    private readonly ITrackingContextFacade _trackingContextFacade;

    public TrackingContextService(ITrackingContextFacade trackingContextFacade)
    {
        _trackingContextFacade = trackingContextFacade;
    }

    public async Task<bool> CanOrderChangeStatusAsync(long orderId, string newStatus)
    {
        return await _trackingContextFacade.CanOrderChangeStatusAsync(orderId, newStatus);
    }

    public async Task<OrderTrackingInfo> GetOrderTrackingInfoAsync(long orderId)
    {
        return await _trackingContextFacade.GetOrderTrackingInfoAsync(orderId);
    }

    public async Task<bool> CanOrderBeDeletedAsync(long orderId)
    {
        return await _trackingContextFacade.CanOrderBeDeletedAsync(orderId);
    }

    public async Task<int> GetActiveContainerCountAsync(long orderId)
    {
        return await _trackingContextFacade.GetActiveContainerCountAsync(orderId);
    }
} 