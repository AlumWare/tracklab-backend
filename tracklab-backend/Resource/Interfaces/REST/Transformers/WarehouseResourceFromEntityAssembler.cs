using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class WarehouseResourceFromEntityAssembler
{
    public static WarehouseResource ToResourceFromEntity(Warehouse warehouse)
    {
        return new WarehouseResource(
            warehouse.Id,
            warehouse.Name,
            warehouse.Type.ToString(),
            warehouse.Coordinates.Latitude,
            warehouse.Coordinates.Longitude,
            warehouse.Address.Value
        );
    }
}