using TrackLab.Shared.Domain.Events;
using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Order.Domain.Events;

/// <summary>
/// Event raised when an order status changes
/// </summary>
public class OrderStatusChangedEvent : DomainEventBase
{
    /// <summary>
    /// Order ID
    /// </summary>
    public long OrderId { get; private set; }
    
    /// <summary>
    /// Previous status
    /// </summary>
    public OrderStatus PreviousStatus { get; private set; }
    
    /// <summary>
    /// New status
    /// </summary>
    public OrderStatus NewStatus { get; private set; }
    
    /// <summary>
    /// Customer tenant ID
    /// </summary>
    public long CustomerId { get; private set; }
    
    /// <summary>
    /// Logistics company tenant ID
    /// </summary>
    public long LogisticsId { get; private set; }
    
    /// <summary>
    /// Reason for the status change
    /// </summary>
    public string? Reason { get; private set; }

    public OrderStatusChangedEvent(
        long orderId,
        OrderStatus previousStatus,
        OrderStatus newStatus,
        long customerId,
        long logisticsId,
        string? reason = null,
        int version = 1) : base(version)
    {
        OrderId = orderId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        CustomerId = customerId;
        LogisticsId = logisticsId;
        Reason = reason;
    }
} 