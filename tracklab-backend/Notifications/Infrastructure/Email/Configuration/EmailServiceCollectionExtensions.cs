using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackLab.Notifications.Infrastructure.Email.Services;

namespace TrackLab.Notifications.Infrastructure.Email.Configuration;

/// <summary>
/// Extensiones para configurar el servicio de correo electrónico
/// </summary>
public static class EmailServiceCollectionExtensions
{
    /// <summary>
    /// Agrega el servicio de correo electrónico a la colección de servicios
    /// </summary>
    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar las opciones de email desde appsettings.json
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        
        // Registrar el servicio de email
        services.AddScoped<IEmailService, SmtpEmailService>();
        
        return services;
    }
} 