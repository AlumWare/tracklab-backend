using Alumware.Tracklab.API.Tracking.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST;

[ApiController]
[Route("api/[controller]")]
[Tags("QR Code")]
public class QrCodeController : ControllerBase
{
    private readonly IQrCodeService _qrCodeService;

    public QrCodeController(IQrCodeService qrCodeService)
    {
        _qrCodeService = qrCodeService;
    }

    [HttpPost("{containerId}")]
    [SwaggerOperation("Generate QR Code for Container")]
    [SwaggerResponse(200, "QR Code generated successfully")]
    [SwaggerResponse(400, "Invalid container ID")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GenerateQrCode(long containerId)
    {
        try
        {
            var qrCode = await _qrCodeService.GenerateQrCodeAsync(containerId, "https://tracklab.com/track");
            
            return Ok(new
            {
                ContainerId = containerId,
                QrCodeUrl = qrCode.Url,
                GeneratedAt = qrCode.GeneratedAt,
                Message = "QR Code generated successfully and uploaded to Cloudinary"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Message = "Error generating QR code",
                Error = ex.Message
            });
        }
    }

    [HttpPost("custom")]
    [SwaggerOperation("Generate Custom QR Code")]
    [SwaggerResponse(200, "QR Code generated successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GenerateCustomQrCode([FromBody] GenerateCustomQrRequest request)
    {
        try
        {
            var qrCode = await _qrCodeService.GenerateQrCodeAsync(request.Data, request.Size);
            
            return Ok(new
            {
                Data = request.Data,
                QrCodeUrl = qrCode.Url,
                GeneratedAt = qrCode.GeneratedAt,
                Message = "Custom QR Code generated successfully"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Message = "Error generating custom QR code",
                Error = ex.Message
            });
        }
    }
}

public record GenerateCustomQrRequest(string Data, int Size = 400); 