using Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;
using Alumware.Tracklab.API.Tracking.Application.Internal.QueryServices;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Infrastructure.Persistence.EFC.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Alumware.Tracklab.API.Tracking.Infrastructure.Configuration;

public static class TrackingServiceCollectionExtensions
{
    public static IServiceCollection AddTrackingServices(this IServiceCollection services)
    {
        // Registrar repositorios
        services.AddScoped<IContainerRepository, ContainerRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<ITrackingEventRepository, TrackingEventRepository>();

        // Registrar servicios de aplicación
        services.AddScoped<IContainerCommandService, ContainerCommandService>();
        services.AddScoped<IContainerQueryService, ContainerQueryService>();
        services.AddScoped<IRouteCommandService, RouteCommandService>();
        services.AddScoped<IRouteQueryService, RouteQueryService>();
        services.AddScoped<ITrackingEventCommandService, TrackingEventCommandService>();
        services.AddScoped<ITrackingEventQueryService, TrackingEventQueryService>();
        return services;
    }
} 