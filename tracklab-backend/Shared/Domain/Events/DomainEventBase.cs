namespace TrackLab.Shared.Domain.Events;

/// <summary>
/// Abstract base class for all domain events
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    /// <summary>
    /// Unique identifier of the event
    /// </summary>
    public Guid EventId { get; private set; }
    
    /// <summary>
    /// Moment when the event occurred
    /// </summary>
    public DateTime OccurredOn { get; private set; }
    
    /// <summary>
    /// Event version (for future evolution)
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Protected constructor to be used by derived classes
    /// </summary>
    /// <param name="version">Event version</param>
    protected DomainEventBase(int version = 1)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        Version = version;
    }
} 