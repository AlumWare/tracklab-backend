using Microsoft.Extensions.DependencyInjection;

namespace TrackLab.Shared.Infrastructure.Multitenancy.Extensions;

/// <summary>
/// Extension methods for configuring tenant context
/// </summary>
public static class TenantContextExtensions
{
    /// <summary>
    /// Add tenant context services to the service collection
    /// </summary>
    public static IServiceCollection AddTenantContext(this IServiceCollection services)
    {
        services.AddScoped<ITenantContext, TenantContext>();
        return services;
    }
} 