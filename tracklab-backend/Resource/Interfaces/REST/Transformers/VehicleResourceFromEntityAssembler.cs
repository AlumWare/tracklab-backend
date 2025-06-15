using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class VehicleResourceFromEntityAssembler
{
    public static VehicleResource ToResourceFromEntity(Vehicle vehicle)
    {
        return new VehicleResource(
            vehicle.Id,
            vehicle.LicensePlate,
            vehicle.LoadCapacity,
            vehicle.PaxCapacity,
            vehicle.Status.ToString(),
            vehicle.Location.Latitude,
            vehicle.Location.Longitude,
            vehicle.ImageAssetIds
        );
    }
}