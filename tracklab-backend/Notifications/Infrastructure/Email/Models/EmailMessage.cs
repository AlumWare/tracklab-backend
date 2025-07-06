using System.Collections.Generic;

namespace TrackLab.Notifications.Infrastructure.Email.Models;

/// <summary>
/// Model that represents an email message
/// </summary>
public class EmailMessage
{
    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public string TextBody { get; set; } = string.Empty;
    public List<string> To { get; set; } = new();
    public List<string> Cc { get; set; } = new();
    public List<string> Bcc { get; set; } = new();
    public List<EmailAttachment> Attachments { get; set; } = new();
}

/// <summary>
/// Model that represents an email attachment
/// </summary>
public class EmailAttachment
{
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "application/octet-stream";
} 