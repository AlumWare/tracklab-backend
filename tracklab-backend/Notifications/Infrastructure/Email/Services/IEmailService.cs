using TrackLab.Notifications.Infrastructure.Email.Models;

namespace TrackLab.Notifications.Infrastructure.Email.Services;

/// <summary>
/// Interface for the email sending service
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email asynchronously
    /// </summary>
    /// <param name="message">The message to send</param>
    /// <returns>True if the sending was successful, False otherwise</returns>
    Task<bool> SendEmailAsync(EmailMessage message);
    
    /// <summary>
    /// Sends a simple email asynchronously
    /// </summary>
    /// <param name="to">Email recipient</param>
    /// <param name="subject">Email subject</param>
    /// <param name="htmlBody">Email body in HTML format</param>
    /// <param name="textBody">Email body in plain text format (optional)</param>
    /// <returns>True if the sending was successful, False otherwise</returns>
    Task<bool> SendSimpleEmailAsync(string to, string subject, string htmlBody, string? textBody = null);
} 