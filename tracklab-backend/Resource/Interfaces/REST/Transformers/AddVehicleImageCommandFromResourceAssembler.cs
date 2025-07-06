using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class AddVehicleImageCommandFromResourceAssembler
{
    public static AddVehicleImageCommand ToCommandFromResource(long vehicleId, AddVehicleImageResource resource)
    {
        return new AddVehicleImageCommand(
            vehicleId,
            resource.ImageFile
        );
    }
} 