using Alumware.Tracklab.API.Tracking.Infrastructure.QrCode.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Alumware.Tracklab.API.Tracking.Infrastructure.Configuration;

public static class TrackingServiceCollectionExtensions
{
    public static IServiceCollection AddTrackingServices(this IServiceCollection services)
    {
        // Add QR Code services
        services.AddQrCodeServices();
        
        return services;
    }
} 