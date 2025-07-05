using Alumware.Tracklab.API.Order.Domain.Repositories;
using Alumware.Tracklab.API.Order.Infrastructure.Persistence.EFC.Repositories;
using Alumware.Tracklab.API.Order.Domain.Services;
using Alumware.Tracklab.API.Order.Application.Internal.CommandServices;

public static class OrderServiceCollectionExtensions
{
    public static IServiceCollection AddOrderModule(this IServiceCollection services)
    {
        services.AddScoped<ITrackingEventRepository, TrackingEventRepository>();
        services.AddScoped<ITrackingEventCommandService, TrackingEventCommandService>();
        return services;
    }
} 