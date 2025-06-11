using TrackLab.Domain.Model.Aggregates;
using TrackLab.Resources.Interfaces.REST.Resources;

namespace TrackLab.Resources.Interfaces.REST.Transformers;

/// <summary>
/// Transformer to convert Warehouse entity to WarehouseResource
/// </summary>
public static class WarehouseResourceFromEntityAssembler
{
    /// <summary>
    /// Transforms a warehouse entity to a warehouse resource
    /// </summary>
    /// <param name="warehouse">Warehouse entity</param>
    /// <returns>Warehouse resource</returns>
    public static WarehouseResource ToResourceFromEntity(Warehouse warehouse)
    {
        return new WarehouseResource(
            warehouse.Id,
            warehouse.TenantId.Value,
            warehouse.Name,
            warehouse.Type.ToString(),
            warehouse.Address.Street,
            warehouse.Address.City,
            warehouse.Address.State,
            warehouse.Address.PostalCode,
            warehouse.Address.Country,
            warehouse.FullAddress,
            warehouse.Location.Latitude,
            warehouse.Location.Longitude,
            warehouse.Phone,
            warehouse.Email);
    }

    /// <summary>
    /// Transforms a collection of warehouse entities to warehouse resources
    /// </summary>
    /// <param name="warehouses">Collection of warehouse entities</param>
    /// <returns>Collection of warehouse resources</returns>
    public static IEnumerable<WarehouseResource> ToResourceFromEntity(IEnumerable<Warehouse> warehouses)
    {
        return warehouses.Select(ToResourceFromEntity);
    }
} 