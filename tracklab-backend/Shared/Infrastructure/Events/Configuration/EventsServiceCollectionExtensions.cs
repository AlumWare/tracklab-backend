using Microsoft.Extensions.DependencyInjection;
using TrackLab.Shared.Domain.Events;
using TrackLab.Shared.Infrastructure.Events;

namespace TrackLab.Shared.Infrastructure.Events.Configuration;

/// <summary>
/// Extensions for configuring the event system in dependency injection
/// </summary>
public static class EventsServiceCollectionExtensions
{
    /// <summary>
    /// Adds the domain events system to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        // Register MediatR if not already registered
        if (!services.Any(service => service.ServiceType == typeof(MediatR.IMediator)))
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(EventsServiceCollectionExtensions).Assembly));
        }
        
        // Register the domain event dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        
        return services;
    }
    
    /// <summary>
    /// Adds the domain events system with specific assembly configuration
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="assemblies">Assemblies where to look for event handlers</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddDomainEvents(this IServiceCollection services, params System.Reflection.Assembly[] assemblies)
    {
        // Register MediatR with specific assemblies
        services.AddMediatR(cfg => 
        {
            foreach (var assembly in assemblies)
            {
                cfg.RegisterServicesFromAssembly(assembly);
            }
        });
        
        // Register the domain event dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        
        return services;
    }
} 