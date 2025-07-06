using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class VehicleResourceFromEntityAssembler
{
    public static VehicleResource ToResourceFromEntity(Vehicle vehicle)
    {
        string statusText = vehicle.Status == EVehicleStatus.Available ? "Available" : "Not available";
        
        var images = vehicle.Images.Select(img => new VehicleImageResource(
            img.Id,
            img.ImageUrl,
            img.PublicId,
            img.CreatedAt
        )).ToList();
        
        return new VehicleResource(
            vehicle.Id,
            vehicle.LicensePlate,
            vehicle.LoadCapacity,
            vehicle.PaxCapacity,
            statusText,
            vehicle.Location.Latitude,
            vehicle.Location.Longitude,
            images
        );
    }
}