using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class CreateVehicleCommandFromResourceAssembler
{
    public static CreateVehicleCommand ToCommandFromResource(CreateVehicleResource resource)
    {
        return new CreateVehicleCommand(
            resource.LicensePlate,
            resource.LoadCapacity,
            resource.PaxCapacity,
            new Coordinates(resource.Latitude, resource.Longitude)
        );
    }
}