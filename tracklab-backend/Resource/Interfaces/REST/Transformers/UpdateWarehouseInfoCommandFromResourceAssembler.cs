using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class UpdateWarehouseInfoCommandFromResourceAssembler
{
    public static UpdateWarehouseInfoCommand ToCommandFromResource(long id, UpdateWarehouseResource resource)
    {
        return new UpdateWarehouseInfoCommand(
            id,
            resource.Name,
            resource.Latitude,
            resource.Longitude,
            resource.Address
        );
    }
}