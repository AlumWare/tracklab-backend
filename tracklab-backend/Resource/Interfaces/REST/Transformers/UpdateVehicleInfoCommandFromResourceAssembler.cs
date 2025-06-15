using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class UpdateVehicleInfoCommandFromResourceAssembler
{
    public static UpdateVehicleInfoCommand ToCommandFromResource(long vehicleId, UpdateVehicleResource resource)
    {
        return new UpdateVehicleInfoCommand(
            vehicleId,
            resource.NewLicensePlate,
            resource.NewLoadCapacity,
            resource.NewPaxCapacity,
            new Coordinates(resource.NewLatitude, resource.NewLongitude)
        );
    }
}