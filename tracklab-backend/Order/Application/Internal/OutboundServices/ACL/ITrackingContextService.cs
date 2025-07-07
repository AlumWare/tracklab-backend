using Alumware.Tracklab.API.Tracking.Interfaces.ACL;

namespace Alumware.Tracklab.API.Order.Application.Internal.OutboundServices.ACL;

public interface ITrackingContextService
{
    /// <summary>
    /// Validates if an order can change its status based on tracking state
    /// </summary>
    Task<bool> CanOrderChangeStatusAsync(long orderId, string newStatus);
    
    /// <summary>
    /// Gets the current tracking progress for an order
    /// </summary>
    Task<OrderTrackingInfo> GetOrderTrackingInfoAsync(long orderId);
    
    /// <summary>
    /// Validates if an order can be deleted (no active containers)
    /// </summary>
    Task<bool> CanOrderBeDeletedAsync(long orderId);
    
    /// <summary>
    /// Gets container count for an order
    /// </summary>
    Task<int> GetActiveContainerCountAsync(long orderId);
} 