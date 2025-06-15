using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class CreateWarehouseCommandFromResourceAssembler
{
    public static CreateWarehouseCommand ToCommandFromResource(CreateWarehouseResource resource)
    {
        Enum.TryParse<EWarehouseType>(resource.Type, out var parsedType);

        return new CreateWarehouseCommand(
            resource.Name,
            parsedType,
            resource.Latitude,
            resource.Longitude,
            resource.Address
        );
    }
}