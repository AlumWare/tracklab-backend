using Alumware.Tracklab.API.Tracking.Domain.Services;
using Alumware.Tracklab.API.Tracking.Infrastructure.QrCode.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Alumware.Tracklab.API.Tracking.Infrastructure.QrCode.Configuration;

public static class QrCodeServiceCollectionExtensions
{
    public static IServiceCollection AddQrCodeServices(this IServiceCollection services)
    {
        services.AddScoped<IQrCodeService, QrCodeService>();
        
        return services;
    }
} 