using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using QRCoder;
using TrackLab.Shared.Domain.Services;

namespace Alumware.Tracklab.API.Tracking.Infrastructure.QrCode.Services;

public class QrCodeService : IQrCodeService
{
    private readonly IImageService _imageService;
    private const int QR_SIZE = 10; // Pixels per module for good quality
    private const string QR_FOLDER = "qr-codes";

    public QrCodeService(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<Domain.Model.ValueObjects.QrCode> GenerateQrCodeAsync(long containerId, string trackingUrl)
    {
        // Generate QR with only container ID
        var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(containerId.ToString(), QRCodeGenerator.ECCLevel.Q);
        var pngQrCode = new PngByteQRCode(qrData);
        
        // Generate PNG bytes directly
        var qrBytes = pngQrCode.GetGraphic(QR_SIZE);
        
        // Upload to Cloudinary
        var fileName = $"container-{containerId}-qr.png";
        var cloudinaryUrl = await _imageService.UploadImageAsync(qrBytes, fileName, QR_FOLDER);
        
        return new Domain.Model.ValueObjects.QrCode(cloudinaryUrl);
    }

    public async Task<Domain.Model.ValueObjects.QrCode> GenerateQrCodeAsync(string trackingUrl, int size = 256)
    {
        // This method generates QR with the full URL
        var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(trackingUrl, QRCodeGenerator.ECCLevel.Q);
        var pngQrCode = new PngByteQRCode(qrData);
        
        var pixelsPerModule = Math.Max(1, size / 25); // Approximate pixels per module
        var qrBytes = pngQrCode.GetGraphic(pixelsPerModule);
        
        var fileName = $"qr-{Guid.NewGuid():N}.png";
        var cloudinaryUrl = await _imageService.UploadImageAsync(qrBytes, fileName, QR_FOLDER);
        
        return new Domain.Model.ValueObjects.QrCode(cloudinaryUrl);
    }

    public bool IsValidQrUrl(string qrUrl)
    {
        if (string.IsNullOrWhiteSpace(qrUrl))
            return false;

        return Uri.TryCreate(qrUrl, UriKind.Absolute, out var uri) && 
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
} 