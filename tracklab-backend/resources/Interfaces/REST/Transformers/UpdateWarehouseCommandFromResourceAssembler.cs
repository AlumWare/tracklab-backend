using TrackLab.Resources.Domain.Model.Commands;
using TrackLab.Resources.Interfaces.REST.Resources;
using TrackLab.Shared.Domain.ValueObjects;

namespace TrackLab.Resources.Interfaces.REST.Transformers;

/// <summary>
/// Transformer to convert UpdateWarehouseResource to UpdateWarehouseCommand
/// </summary>
public static class UpdateWarehouseCommandFromResourceAssembler
{
    /// <summary>
    /// Transforms an update warehouse resource to an update warehouse command
    /// </summary>
    /// <param name="id">Warehouse identifier</param>
    /// <param name="resource">Update warehouse resource</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>Update warehouse command</returns>
    public static UpdateWarehouseCommand ToCommandFromResource(int id, UpdateWarehouseResource resource, TenantId tenantId)
    {
        return new UpdateWarehouseCommand(
            id,
            tenantId,
            resource.Name,
            resource.Type,
            resource.Street,
            resource.City,
            resource.State,
            resource.PostalCode,
            resource.Country,
            resource.Latitude,
            resource.Longitude,
            resource.Phone ?? string.Empty,
            resource.Email ?? string.Empty);
    }
} 