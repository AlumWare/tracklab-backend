using System;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

public record QrCode
{
    public string Url { get; private set; } = string.Empty;
    public DateTime GeneratedAt { get; private set; }

    private QrCode() { }

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