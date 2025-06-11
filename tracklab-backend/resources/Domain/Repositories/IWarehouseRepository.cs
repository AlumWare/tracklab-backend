using TrackLab.Domain.Model.Aggregates;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Resources.Domain.Model.ValueObjects;

namespace TrackLab.Resources.Domain.Repositories;

public interface IWarehouseRepository : IBaseRepository<Warehouse>
{
    /// <summary>
    /// Gets all warehouses for a specific tenant
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>List of warehouses</returns>
    Task<IEnumerable<Warehouse>> GetByTenantIdAsync(TenantId tenantId);
    
    /// <summary>
    /// Gets warehouses by type for a specific tenant
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="type">Warehouse type</param>
    /// <returns>List of warehouses</returns>
    Task<IEnumerable<Warehouse>> GetByTenantIdAndTypeAsync(TenantId tenantId, WarehouseType type);
    
    /// <summary>
    /// Gets a warehouse by id and tenant id
    /// </summary>
    /// <param name="id">Warehouse identifier</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>Warehouse or null if not found</returns>
    Task<Warehouse?> GetByIdAndTenantIdAsync(int id, TenantId tenantId);
}