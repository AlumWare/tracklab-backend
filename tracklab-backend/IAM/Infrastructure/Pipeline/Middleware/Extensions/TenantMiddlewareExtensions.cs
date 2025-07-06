using TrackLab.IAM.Infrastructure.Pipeline.Middleware.Components;
using Alumware.Tracklab.API.IAM.Infrastructure.Pipeline.Middleware.Components;

namespace TrackLab.IAM.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
/// Extension methods for registering tenant middleware
/// </summary>
public static class TenantMiddlewareExtensions
{
    /// <summary>
    /// Add tenant resolution middleware to the pipeline
    /// </summary>
    public static IApplicationBuilder UseTenantResolution(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantResolutionMiddleware>();
    }
    
    /// <summary>
    /// Add tenant context services to DI container
    /// </summary>
    public static IServiceCollection AddTenantContext(this IServiceCollection services)
    {
        // Register the TenantContext as singleton since it uses AsyncLocal
        services.AddSingleton<TrackLab.Shared.Infrastructure.Multitenancy.ITenantContext, 
                              TrackLab.Shared.Infrastructure.Multitenancy.TenantContext>();
        
        return services;
    }

    /// <summary>
    /// Add exception handling middleware to the pipeline
    /// </summary>
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
} 