using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TrackLab.Shared.Domain.Exceptions;
using TrackLab.Shared.Domain.Services;
using TrackLab.Shared.Infrastructure.Images.Cloudinary.Configuration;

namespace TrackLab.Shared.Infrastructure.Images.Cloudinary.Services;

public class CloudinaryImageService : IImageService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;
    private readonly CloudinarySettings _settings;
    private readonly ILogger<CloudinaryImageService> _logger;

    public CloudinaryImageService(
        IOptions<CloudinarySettings> settings,
        ILogger<CloudinaryImageService> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        if (string.IsNullOrEmpty(_settings.CloudName) || 
            string.IsNullOrEmpty(_settings.ApiKey) || 
            string.IsNullOrEmpty(_settings.ApiSecret))
        {
            throw new ImageServiceException("Cloudinary settings are not properly configured");
        }

        var account = new Account(_settings.CloudName, _settings.ApiKey, _settings.ApiSecret);
        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string? folder = null)
    {
        try
        {
            _logger.LogInformation("Starting image upload: {FileName}", fileName);

            ValidateImageStream(imageStream);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, imageStream),
                Folder = folder ?? _settings.DefaultFolder,
                PublicId = $"{folder ?? _settings.DefaultFolder}/{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.UtcNow:yyyyMMddHHmmss}",
                Overwrite = false,
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ImageUploadException($"Error uploading image: {uploadResult.Error?.Message}");
            }

            _logger.LogInformation("Image uploaded successfully: {Url}", uploadResult.SecureUrl);
            return uploadResult.SecureUrl.ToString();
        }
        catch (Exception ex) when (!(ex is ImageServiceException))
        {
            _logger.LogError(ex, "Unexpected error uploading image: {FileName}", fileName);
            throw new ImageUploadException($"Error uploading image: {ex.Message}", ex);
        }
    }

    public async Task<string> UploadImageAsync(byte[] imageBytes, string fileName, string? folder = null)
    {
        using var imageStream = new MemoryStream(imageBytes);
        return await UploadImageAsync(imageStream, fileName, folder);
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        try
        {
            _logger.LogInformation("Starting image deletion: {PublicId}", publicId);

            var deleteParams = new DeletionParams(publicId);

            var deletionResult = await _cloudinary.DestroyAsync(deleteParams);

            if (deletionResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ImageDeleteException($"Error deleting image: {deletionResult.Error?.Message}");
            }

            var success = deletionResult.Result == "ok";
            _logger.LogInformation("Image deleted: {PublicId}, Success: {Success}", publicId, success);
            return success;
        }
        catch (Exception ex) when (!(ex is ImageServiceException))
        {
            _logger.LogError(ex, "Unexpected error deleting image: {PublicId}", publicId);
            throw new ImageDeleteException($"Error deleting image: {ex.Message}", ex);
        }
    }

    public string ExtractPublicIdFromUrl(string imageUrl)
    {
        try
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new InvalidImageUrlException("The image URL cannot be empty");
            }

            var uri = new Uri(imageUrl);
            var path = uri.AbsolutePath;

            // Remove version prefix if exists (e.g., /v1234567890/)
            var versionPattern = @"^/v\d+/";
            path = System.Text.RegularExpressions.Regex.Replace(path, versionPattern, "/");

            // Remove the first '/' and file extension
            path = path.TrimStart('/');
            var dotIndex = path.LastIndexOf('.');
            if (dotIndex > 0)
            {
                path = path.Substring(0, dotIndex);
            }

            return path;
        }
        catch (Exception ex) when (!(ex is ImageServiceException))
        {
            throw new InvalidImageUrlException($"Invalid image URL: {ex.Message}");
        }
    }

    private void ValidateImageStream(Stream imageStream)
    {
        if (imageStream == null || imageStream.Length == 0)
        {
            throw new ImageUploadException("The image stream cannot be empty");
        }

        if (imageStream.Length > _settings.MaxImageSizeInBytes)
        {
            throw new ImageUploadException($"The image size exceeds the maximum limit of {_settings.MaxImageSizeInBytes / (1024 * 1024)} MB");
        }

        // Reset stream position for reading
        if (imageStream.CanSeek)
        {
            imageStream.Position = 0;
        }
    }
} 