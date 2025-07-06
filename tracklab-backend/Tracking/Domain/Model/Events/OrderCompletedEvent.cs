using TrackLab.Shared.Domain.Events;

namespace Alumware.Tracklab.API.Tracking.Domain.Events;

/// <summary>
/// Event raised when an order is completed (all containers delivered)
/// </summary>
public class OrderCompletedEvent : DomainEventBase
{
    /// <summary>
    /// Order ID
    /// </summary>
    public long OrderId { get; private set; }
    
    /// <summary>
    /// Customer tenant ID
    /// </summary>
    public long CustomerId { get; private set; }
    
    /// <summary>
    /// Logistics company tenant ID
    /// </summary>
    public long LogisticsId { get; private set; }
    
    /// <summary>
    /// Total number of containers delivered
    /// </summary>
    public int TotalContainers { get; private set; }
    
    /// <summary>
    /// Total weight of all containers
    /// </summary>
    public decimal TotalWeight { get; private set; }
    
    /// <summary>
    /// Completion timestamp
    /// </summary>
    public DateTime CompletedAt { get; private set; }
    
    /// <summary>
    /// Delivery statistics
    /// </summary>
    public string DeliveryStats { get; private set; }

    public OrderCompletedEvent(
        long orderId,
        long customerId,
        long logisticsId,
        int totalContainers,
        decimal totalWeight,
        DateTime completedAt,
        string deliveryStats,
        int version = 1) : base(version)
    {
        OrderId = orderId;
        CustomerId = customerId;
        LogisticsId = logisticsId;
        TotalContainers = totalContainers;
        TotalWeight = totalWeight;
        CompletedAt = completedAt;
        DeliveryStats = deliveryStats;
    }
} 