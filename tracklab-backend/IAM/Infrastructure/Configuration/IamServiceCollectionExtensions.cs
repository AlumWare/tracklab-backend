using Microsoft.Extensions.DependencyInjection;
using TrackLab.IAM.Application.Internal.CommandServices;
using TrackLab.IAM.Application.Internal.OutboundServices;
using TrackLab.IAM.Application.Internal.QueryServices;
using TrackLab.IAM.Domain.Repositories;
using TrackLab.IAM.Domain.Services;
using TrackLab.IAM.Infrastructure.Hashing.BCrypt.Services;
using TrackLab.IAM.Infrastructure.Persistence.EFC.Repositories;
using TrackLab.IAM.Infrastructure.Tokens.JWT.Services;

namespace TrackLab.IAM.Infrastructure.Configuration;

/// <summary>
/// Extension methods for configuring IAM services
/// </summary>
public static class IamServiceCollectionExtensions
{
    /// <summary>
    /// Add IAM services to the service collection
    /// </summary>
    public static IServiceCollection AddIamServices(this IServiceCollection services)
    {
        // Domain Services
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<IUserQueryService, UserQueryService>();
        
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        
        // Infrastructure Services
        services.AddScoped<IHashingService, HashingService>();
        services.AddScoped<ITokenService, TokenService>();
        
        return services;
    }
} 