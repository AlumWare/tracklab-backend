namespace TrackLab.Shared.Infrastructure.Images.Cloudinary.Configuration;

public class CloudinarySettings
{
    public const string SectionName = "Cloudinary";
    
    public string CloudName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
    public string SecureUrl { get; set; } = "https://res.cloudinary.com";
    public int MaxImageSizeInBytes { get; set; } = 5 * 1024 * 1024; // 5MB default
    public string[] AllowedFormats { get; set; } = { "jpg", "jpeg", "png", "gif", "webp" };
    public string DefaultFolder { get; set; } = "tracklab";
} 