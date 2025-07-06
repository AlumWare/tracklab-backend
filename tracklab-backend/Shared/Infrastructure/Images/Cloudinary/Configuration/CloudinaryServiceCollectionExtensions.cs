using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackLab.Shared.Domain.Services;
using TrackLab.Shared.Infrastructure.Images.Cloudinary.Services;

namespace TrackLab.Shared.Infrastructure.Images.Cloudinary.Configuration;

public static class CloudinaryServiceCollectionExtensions
{
    public static IServiceCollection AddCloudinaryImageService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure Cloudinary options
        services.Configure<CloudinarySettings>(
            configuration.GetSection(CloudinarySettings.SectionName));

        // Register image service
        services.AddScoped<IImageService, CloudinaryImageService>();

        return services;
    }
} 