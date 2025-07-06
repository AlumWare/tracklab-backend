namespace Alumware.Tracklab.API.Resource.Domain.Model.Entities;

public class VehicleImage
{
    public long Id { get; private set; }
    public long VehicleId { get; private set; }
    public string ImageUrl { get; private set; } = null!;
    public string PublicId { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    
    public VehicleImage() { }
    
    public VehicleImage(long vehicleId, string imageUrl, string publicId)
    {
        VehicleId = vehicleId;
        ImageUrl = imageUrl;
        PublicId = publicId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void UpdateImageUrl(string newImageUrl, string newPublicId)
    {
        ImageUrl = newImageUrl;
        PublicId = newPublicId;
    }
}
