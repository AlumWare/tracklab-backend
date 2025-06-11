using TrackLab.Shared.Domain.ValueObjects;

namespace TrackLab.Resources.Domain.Model.Queries;

/// <summary>
/// Query to get all warehouses for a tenant
/// </summary>
/// <param name="TenantId">Tenant identifier</param>
public record GetAllWarehousesQuery(TenantId TenantId); 