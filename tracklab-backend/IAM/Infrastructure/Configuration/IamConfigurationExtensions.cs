using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrackLab.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using TrackLab.IAM.Infrastructure.Tokens.JWT.Configuration;

namespace TrackLab.IAM.Infrastructure.Configuration;

/// <summary>
/// Extension methods for configuring complete IAM infrastructure
/// </summary>
public static class IamConfigurationExtensions
{
    /// <summary>
    /// Add complete IAM configuration including JWT, services, and multitenancy
    /// </summary>
    public static IServiceCollection AddIamConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Add basic IAM services
        services.AddIamServices();
        
        // Add tenant context
        services.AddTenantContext();
        
        // Add JWT configuration
        var tokenSettings = new TokenSettings();
        configuration.GetSection(TokenSettings.SectionName).Bind(tokenSettings);
        services.AddSingleton(tokenSettings);
        
        // Add JWT authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenSettings.Issuer,
                    ValidAudience = tokenSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        return services;
    }
    
    /// <summary>
    /// Configure the IAM middleware pipeline
    /// </summary>
    public static IApplicationBuilder UseIamConfiguration(this IApplicationBuilder app)
    {
        // Add authentication middleware
        app.UseAuthentication();
        
        // Add authorization middleware (includes tenant resolution)
        app.UseRequestAuthorization();
        
        return app;
    }
} 