using Microsoft.AspNetCore.Mvc;
using TrackLab.Shared.Domain.Exceptions;
using TrackLab.Shared.Domain.Services;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using TrackLab.Shared.Interfaces.REST.Resources;

namespace TrackLab.Shared.Interfaces.REST;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly ILogger<ImageController> _logger;

    public ImageController(IImageService imageService, ILogger<ImageController> logger)
    {
        _imageService = imageService;
        _logger = logger;
    }

    /// <summary>
    /// Upload a single image file
    /// </summary>
    /// <param name="resource">The upload image resource containing the file and optional folder</param>
    /// <returns>Upload result with image URL and details</returns>
    /// <response code="200">Returns the uploaded image details</response>
    /// <response code="400">If the file is invalid or empty</response>
    /// <response code="500">If there was an internal error during upload</response>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageResource resource)
    {
        try
        {
            if (resource.File == null || resource.File.Length == 0)
            {
                return BadRequest(new { error = "No file provided" });
            }

            _logger.LogInformation("Uploading image: {FileName}, Size: {FileSize} bytes", resource.File.FileName, resource.File.Length);

            using var stream = resource.File.OpenReadStream();
            var imageUrl = await _imageService.UploadImageAsync(stream, resource.File.FileName);

            return Ok(new 
            { 
                message = "Image uploaded successfully",
                imageUrl = imageUrl,
                fileName = resource.File.FileName,
                fileSize = resource.File.Length,
                folder = resource.Folder ?? "tracklab"
            });
        }
        catch (ImageUploadException ex)
        {
            _logger.LogError(ex, "Error uploading image: {FileName}", resource.File?.FileName);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error uploading image: {FileName}", resource.File?.FileName);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete an image by public ID
    /// </summary>
    /// <param name="publicId">The public ID of the image to delete</param>
    /// <returns>Deletion confirmation</returns>
    /// <response code="200">Image was successfully deleted</response>
    /// <response code="400">If the public ID is invalid</response>
    /// <response code="404">If the image was not found</response>
    /// <response code="500">If there was an internal error during deletion</response>
    [HttpDelete("{publicId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteImage(string publicId)
    {
        try
        {
            if (string.IsNullOrEmpty(publicId))
            {
                return BadRequest(new { error = "Public ID is required" });
            }

            _logger.LogInformation("Deleting image: {PublicId}", publicId);

            var success = await _imageService.DeleteImageAsync(publicId);

            if (success)
            {
                return Ok(new { message = "Image deleted successfully", publicId = publicId });
            }
            else
            {
                return NotFound(new { error = "Image not found", publicId = publicId });
            }
        }
        catch (ImageDeleteException ex)
        {
            _logger.LogError(ex, "Error deleting image: {PublicId}", publicId);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting image: {PublicId}", publicId);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Extract public ID from Cloudinary URL
    /// </summary>
    /// <param name="imageUrl">The Cloudinary URL to extract the public ID from</param>
    /// <returns>The extracted public ID</returns>
    /// <response code="200">Returns the extracted public ID</response>
    /// <response code="400">If the URL is invalid or empty</response>
    /// <response code="500">If there was an internal error during extraction</response>
    [HttpGet("extract-public-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ExtractPublicId([FromQuery] string imageUrl)
    {
        try
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return BadRequest(new { error = "Image URL is required" });
            }

            var publicId = _imageService.ExtractPublicIdFromUrl(imageUrl);

            return Ok(new { publicId = publicId, imageUrl = imageUrl });
        }
        catch (InvalidImageUrlException ex)
        {
            _logger.LogError(ex, "Invalid image URL: {ImageUrl}", imageUrl);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error extracting public ID: {ImageUrl}", imageUrl);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Health check endpoint for the image service
    /// </summary>
    /// <returns>Service health status</returns>
    /// <response code="200">Service is healthy</response>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new { message = "Image service is healthy", timestamp = DateTime.UtcNow });
    }
} 