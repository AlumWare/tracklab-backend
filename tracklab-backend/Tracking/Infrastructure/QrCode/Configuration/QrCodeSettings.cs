namespace Alumware.Tracklab.API.Tracking.Infrastructure.QrCode.Configuration;

public class QrCodeSettings
{
    public string BaseUrl { get; set; } = "https://api.qrserver.com/v1/create-qr-code/";
    public int DefaultSize { get; set; } = 256;
    public int TimeoutSeconds { get; set; } = 30;
} 