using TrackLab.Shared.Domain.ValueObjects;

namespace TrackLab.Resources.Domain.Model.Commands;

/// <summary>
/// Command to delete a warehouse
/// </summary>
/// <param name="Id">Warehouse identifier</param>
/// <param name="TenantId">Tenant identifier</param>
public record DeleteWarehouseCommand(int Id, TenantId TenantId); 