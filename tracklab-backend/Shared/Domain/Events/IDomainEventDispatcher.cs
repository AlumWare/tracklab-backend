namespace TrackLab.Shared.Domain.Events;

/// <summary>
/// Interface for domain event dispatcher
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Publishes a domain event asynchronously
    /// </summary>
    /// <param name="domainEvent">Domain event to publish</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task PublishAsync(IDomainEvent domainEvent);
    
    /// <summary>
    /// Publishes multiple domain events asynchronously
    /// </summary>
    /// <param name="domainEvents">Collection of domain events to publish</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task PublishAsync(IEnumerable<IDomainEvent> domainEvents);
} 