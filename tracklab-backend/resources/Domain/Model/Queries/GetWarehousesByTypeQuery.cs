using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Resources.Domain.Model.ValueObjects;

namespace TrackLab.Resources.Domain.Model.Queries;

/// <summary>
/// Query to get warehouses by type for a tenant
/// </summary>
/// <param name="TenantId">Tenant identifier</param>
/// <param name="Type">Warehouse type</param>
public record GetWarehousesByTypeQuery(TenantId TenantId, WarehouseType Type); 