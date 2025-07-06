using MediatR;

namespace TrackLab.Shared.Domain.Events;

/// <summary>
/// Base interface for all domain events
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Unique identifier of the event
    /// </summary>
    Guid EventId { get; }
    
    /// <summary>
    /// Moment when the event occurred
    /// </summary>
    DateTime OccurredOn { get; }
    
    /// <summary>
    /// Event version (for future evolution)
    /// </summary>
    int Version { get; }
} 