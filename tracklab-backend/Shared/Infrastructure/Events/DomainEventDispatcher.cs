using MediatR;
using Microsoft.Extensions.Logging;
using TrackLab.Shared.Domain.Events;

namespace TrackLab.Shared.Infrastructure.Events;

/// <summary>
/// Domain event dispatcher implementation using MediatR
/// </summary>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Publishes a domain event asynchronously
    /// </summary>
    /// <param name="domainEvent">Domain event to publish</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public async Task PublishAsync(IDomainEvent domainEvent)
    {
        try
        {
            _logger.LogInformation("Publishing domain event: {EventType} with Id: {EventId}", 
                domainEvent.GetType().Name, domainEvent.EventId);
            
            await _mediator.Publish(domainEvent);
            
            _logger.LogInformation("Successfully published domain event: {EventType} with Id: {EventId}", 
                domainEvent.GetType().Name, domainEvent.EventId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing domain event: {EventType} with Id: {EventId}",
                domainEvent.GetType().Name, domainEvent.EventId);
            throw;
        }
    }

    /// <summary>
    /// Publishes multiple domain events asynchronously
    /// </summary>
    /// <param name="domainEvents">Collection of domain events to publish</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public async Task PublishAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        var events = domainEvents.ToList();
        
        if (!events.Any())
        {
            _logger.LogDebug("No domain events to publish");
            return;
        }

        _logger.LogInformation("Publishing {EventCount} domain events", events.Count);

        try
        {
            // Publish all events in parallel
            var publishTasks = events.Select(PublishAsync);
            await Task.WhenAll(publishTasks);
            
            _logger.LogInformation("Successfully published {EventCount} domain events", events.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing domain events");
            throw;
        }
    }
}