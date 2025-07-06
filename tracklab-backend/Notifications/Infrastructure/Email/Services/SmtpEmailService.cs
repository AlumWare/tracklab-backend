using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using TrackLab.Notifications.Infrastructure.Email.Configuration;
using TrackLab.Notifications.Infrastructure.Email.Models;

namespace TrackLab.Notifications.Infrastructure.Email.Services;

/// <summary>
/// Implementación del servicio de correo electrónico usando SMTP a través de MailKit
/// </summary>
public class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptions<EmailSettings> settings, ILogger<SmtpEmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(EmailMessage message)
    {
        try
        {
            var email = new MimeMessage();

            // Configurar remitente
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));

            // Configurar destinatarios
            foreach (var to in message.To)
            {
                email.To.Add(MailboxAddress.Parse(to));
            }

            foreach (var cc in message.Cc)
            {
                email.Cc.Add(MailboxAddress.Parse(cc));
            }

            foreach (var bcc in message.Bcc)
            {
                email.Bcc.Add(MailboxAddress.Parse(bcc));
            }

            // Configurar asunto
            email.Subject = message.Subject;

            // Crear el cuerpo del mensaje
            var builder = new BodyBuilder();

            if (!string.IsNullOrEmpty(message.HtmlBody))
            {
                builder.HtmlBody = message.HtmlBody;
            }

            if (!string.IsNullOrEmpty(message.TextBody))
            {
                builder.TextBody = message.TextBody;
            }

            // Agregar adjuntos si existen
            foreach (var attachment in message.Attachments)
            {
                builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.ContentType));
            }

            email.Body = builder.ToMessageBody();

            // Enviar el correo
            using var client = new SmtpClient();
            
            await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, 
                _settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
            
            if (!string.IsNullOrEmpty(_settings.SmtpUsername))
            {
                await client.AuthenticateAsync(_settings.SmtpUsername, _settings.SmtpPassword);
            }

            await client.SendAsync(email);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Correo enviado exitosamente a {Recipients}", string.Join(", ", message.To));
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar correo a {Recipients}", string.Join(", ", message.To));
            return false;
        }
    }

    public async Task<bool> SendSimpleEmailAsync(string to, string subject, string htmlBody, string? textBody = null)
    {
        var message = new EmailMessage
        {
            Subject = subject,
            HtmlBody = htmlBody,
            TextBody = textBody ?? string.Empty,
            To = new List<string> { to }
        };

        return await SendEmailAsync(message);
    }
} 