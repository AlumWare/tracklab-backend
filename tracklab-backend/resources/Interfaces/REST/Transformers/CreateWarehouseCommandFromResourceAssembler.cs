using TrackLab.Resources.Domain.Model.Commands;
using TrackLab.Resources.Interfaces.REST.Resources;
using TrackLab.Shared.Domain.ValueObjects;

namespace TrackLab.Resources.Interfaces.REST.Transformers;

/// <summary>
/// Transformer to convert CreateWarehouseResource to CreateWarehouseCommand
/// </summary>
public static class CreateWarehouseCommandFromResourceAssembler
{
    /// <summary>
    /// Transforms a create warehouse resource to a create warehouse command
    /// </summary>
    /// <param name="resource">Create warehouse resource</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>Create warehouse command</returns>
    public static CreateWarehouseCommand ToCommandFromResource(CreateWarehouseResource resource, TenantId tenantId)
    {
        return new CreateWarehouseCommand(
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