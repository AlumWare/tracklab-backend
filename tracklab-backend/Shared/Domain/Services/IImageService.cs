namespace TrackLab.Shared.Domain.Services;

public interface IImageService
{
    /// <summary>
    /// Uploads an image to Cloudinary and returns the URL
    /// </summary>
    /// <param name="imageStream">Stream of the image to upload</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="folder">Folder where the image will be stored (optional)</param>
    /// <returns>URL of the uploaded image</returns>
    Task<string> UploadImageAsync(Stream imageStream, string fileName, string? folder = null);

    /// <summary>
    /// Uploads an image from a byte array
    /// </summary>
    /// <param name="imageBytes">Byte array of the image</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="folder">Folder where the image will be stored (optional)</param>
    /// <returns>URL of the uploaded image</returns>
    Task<string> UploadImageAsync(byte[] imageBytes, string fileName, string? folder = null);

    /// <summary>
    /// Deletes an image from Cloudinary
    /// </summary>
    /// <param name="publicId">Public ID of the image in Cloudinary</param>
    /// <returns>True if deleted successfully</returns>
    Task<bool> DeleteImageAsync(string publicId);

    /// <summary>
    /// Extracts the public ID from a Cloudinary URL
    /// </summary>
    /// <param name="imageUrl">URL of the image in Cloudinary</param>
    /// <returns>Public ID of the image</returns>
    string ExtractPublicIdFromUrl(string imageUrl);
} 