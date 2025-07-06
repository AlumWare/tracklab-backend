using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Tracking.Domain.Services;

public interface IQrCodeService
{
    /// <summary>
    /// Generates a QR code for a specific container
    /// </summary>
    /// <param name="containerId">Container ID</param>
    /// <param name="trackingUrl">Base URL for tracking</param>
    /// <returns>QrCode Value Object with generated URL</returns>
    Task<QrCode> GenerateQrCodeAsync(long containerId, string trackingUrl);

    /// <summary>
    /// Generates a QR code with custom parameters
    /// </summary>
    /// <param name="trackingUrl">Complete tracking URL</param>
    /// <param name="size">QR size (optional, default 256x256)</param>
    /// <returns>QrCode Value Object with generated URL</returns>
    Task<QrCode> GenerateQrCodeAsync(string trackingUrl, int size = 256);

    /// <summary>
    /// Validates if a QR URL is valid
    /// </summary>
    /// <param name="qrUrl">QR URL to validate</param>
    /// <returns>True if the URL is valid</returns>
    bool IsValidQrUrl(string qrUrl);
} 