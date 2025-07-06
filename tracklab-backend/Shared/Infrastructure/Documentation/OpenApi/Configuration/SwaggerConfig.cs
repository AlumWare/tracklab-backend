using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace TrackLab.Shared.Infrastructure.Documentation.OpenApi.Configuration;

/// <summary>
/// Swagger configuration for TrackLab API with JWT support
/// </summary>
public static class SwaggerConfig
{
    /// <summary>
    /// Configure Swagger/OpenAPI with JWT Bearer authentication
    /// </summary>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // API Information
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "TrackLab API",
                Description = "API multitenant para gestión logística con autenticación JWT",
                Contact = new OpenApiContact
                {
                    Name = "TrackLab Team",
                    Email = "support@tracklab.com"
                }
            });

            // Custom schema IDs
            options.CustomSchemaIds(type => type.FullName);

            // JWT Bearer Authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingrese 'Bearer' seguido de un espacio y luego su token JWT.\n\nEjemplo: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
            });

            // Global security requirement
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Include XML comments if available
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            // Custom operation filters
            options.OperationFilter<AuthorizeOperationFilter>();
        });

        return services;
    }
}

/// <summary>
/// Operation filter to handle authorization documentation
/// </summary>
public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Check if the endpoint has [AllowAnonymous] attribute
        var allowAnonymous = context.MethodInfo.GetCustomAttributes(true)
            .Union(context.MethodInfo.DeclaringType?.GetCustomAttributes(true) ?? Array.Empty<object>())
            .OfType<TrackLab.IAM.Infrastructure.Pipeline.Middleware.Attributes.AllowAnonymousAttribute>()
            .Any();

        if (allowAnonymous)
        {
            // Remove security requirements for anonymous endpoints
            operation.Security?.Clear();
            
            // Add description for anonymous endpoints
            if (operation.Summary != null)
            {
                operation.Summary += " (No requiere autenticación)";
            }
        }
        else
        {
            // Add description for protected endpoints
            if (operation.Summary != null)
            {
                operation.Summary += " (Requiere autenticación JWT)";
            }
            
            // Ensure security requirement is present
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            
            if (!operation.Security.Any())
            {
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            }
        }
    }
}
