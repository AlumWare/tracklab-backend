using TrackLab.Shared.Domain.ValueObjects;

namespace TrackLab.Resources.Domain.Model.Queries;

/// <summary>
/// Query to get a warehouse by its identifier
/// </summary>
/// <param name="Id">Warehouse identifier</param>
/// <param name="TenantId">Tenant identifier</param>
public record GetWarehouseByIdQuery(int Id, TenantId TenantId); 