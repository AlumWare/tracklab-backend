namespace Alumware.Tracklab.API.Tracking.Interfaces.ACL;

/// <summary>
/// Facade interface exposed by TrackingContext for OrderContext integration
/// </summary>
public interface ITrackingContextFacade
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

/// <summary>
/// Tracking information for an order
/// </summary>
public record OrderTrackingInfo(
    long OrderId,
    int TotalContainers,
    int DeliveredContainers,
    int InTransitContainers,
    int PendingContainers,
    string OverallStatus,
    DateTime? LastUpdated
); 