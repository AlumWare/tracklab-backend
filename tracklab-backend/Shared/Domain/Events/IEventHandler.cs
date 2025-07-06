using MediatR;

namespace TrackLab.Shared.Domain.Events;

/// <summary>
/// Base interface for domain event handlers
/// </summary>
/// <typeparam name="TDomainEvent">Type of domain event that this handler manages</typeparam>
public interface IEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    // We inherit from INotificationHandler<TDomainEvent> to leverage MediatR
    // We don't need additional methods, but we keep the interface
    // for consistency and possible future extensions
} 