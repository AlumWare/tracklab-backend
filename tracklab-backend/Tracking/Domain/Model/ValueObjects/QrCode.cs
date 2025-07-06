using System;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

public record QrCode
{
    public string Url { get; }
    public DateTime GeneratedAt { get; }

    public QrCode(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("QR URL cannot be empty", nameof(url));

        Url = url;
        GeneratedAt = DateTime.UtcNow;
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(Url);
    
    public override string ToString() => $"QR: {Url}";
} 