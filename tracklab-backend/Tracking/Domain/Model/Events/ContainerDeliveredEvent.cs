using TrackLab.Shared.Domain.Events;

namespace Alumware.Tracklab.API.Tracking.Domain.Events;

/// <summary>
/// Event raised when a container is delivered
/// </summary>
public class ContainerDeliveredEvent : DomainEventBase
{
    /// <summary>
    /// Container ID
    /// </summary>
    public long ContainerId { get; private set; }
    
    /// <summary>
    /// Order ID associated with the container
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
    /// Delivery location
    /// </summary>
    public string DeliveryLocation { get; private set; }
    
    /// <summary>
    /// Delivered at timestamp
    /// </summary>
    public DateTime DeliveredAt { get; private set; }
    
    /// <summary>
    /// Total weight of the container
    /// </summary>
    public decimal TotalWeight { get; private set; }

    public ContainerDeliveredEvent(
        long containerId,
        long orderId,
        long customerId,
        long logisticsId,
        string deliveryLocation,
        DateTime deliveredAt,
        decimal totalWeight,
        int version = 1) : base(version)
    {
        ContainerId = containerId;
        OrderId = orderId;
        CustomerId = customerId;
        LogisticsId = logisticsId;
        DeliveryLocation = deliveryLocation;
        DeliveredAt = deliveredAt;
        TotalWeight = totalWeight;
    }
} 