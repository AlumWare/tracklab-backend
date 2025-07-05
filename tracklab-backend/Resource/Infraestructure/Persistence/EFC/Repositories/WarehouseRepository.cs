using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Alumware.Tracklab.API.Resource.Infrastructure.Persistence.Repositories;

/**
 * <summary>
 *     The warehouse repository
 * </summary>
 * <remarks>
 *     This repository is used to manage warehouses with tenant awareness
 * </remarks>
 */
public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
{
    private readonly ITenantContext _tenantContext;

    public WarehouseRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    /// <summary>
    /// Get queryable with tenant filter applied
    /// </summary>
    private IQueryable<Warehouse> GetTenantFilteredQuery()
    {
        var query = Context.Set<Warehouse>().AsQueryable();
        
        if (_tenantContext.HasTenant)
        {
            var currentTenantId = _tenantContext.CurrentTenantId!.Value;
            query = query.Where(w => w.TenantId == currentTenantId);
        }
        
        return query;
    }

    public new async Task<IEnumerable<Warehouse>> ListAsync()
    {
        return await GetTenantFilteredQuery()
            .OrderBy(w => w.Name)
            .ToListAsync();
    }

    public new async Task<Warehouse?> FindByIdAsync(long id)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public Warehouse SaveAsync(Warehouse warehouse)
    {
        // Ensure tenant is set if creating new warehouse
        if (warehouse.Id == 0 && _tenantContext.HasTenant && warehouse.TenantId == 0)
        {
            warehouse.SetTenantId(_tenantContext.CurrentTenantId!.Value);
        }

        if (warehouse.Id == 0)
        {
            AddAsync(warehouse).Wait();
        }
        else
        {
            Update(warehouse);
        }

        return warehouse;
    }

    public void DeleteAsync(Warehouse warehouse)
    {
        // Verify warehouse belongs to current tenant for security
        if (_tenantContext.HasTenant && warehouse.TenantId != _tenantContext.CurrentTenantId!.Value)
        {
            throw new UnauthorizedAccessException("Cannot delete warehouse from different tenant");
        }

        Remove(warehouse);
    }

    public async Task<(IEnumerable<Warehouse> Warehouses, int TotalCount)> FindPaginatedAsync(int page, int size, string? searchTerm = null)
    {
        var query = GetTenantFilteredQuery();

        // Apply search filter if provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(w => 
                w.Name.ToLower().Contains(searchTerm) ||
                w.Address.ToString().ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        
        var warehouses = await query
            .OrderBy(w => w.Name)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (warehouses, totalCount);
    }

    public async Task<IEnumerable<Warehouse>> FindByTypeAsync(string type)
    {
        if (Enum.TryParse<Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects.EWarehouseType>(type, out var typeEnum))
        {
            return await GetTenantFilteredQuery()
                .Where(w => w.Type == typeEnum)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }
        
        return Enumerable.Empty<Warehouse>();
    }

    public async Task<IEnumerable<Warehouse>> FindByNameAsync(string name)
    {
        return await GetTenantFilteredQuery()
            .Where(w => w.Name.ToLower().Contains(name.ToLower()))
            .OrderBy(w => w.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Warehouse>> FindByLocationAsync(string location)
    {
        return await GetTenantFilteredQuery()
            .Where(w => 
                w.Address.ToString().ToLower().Contains(location.ToLower()) ||
                w.Coordinates.ToString().ToLower().Contains(location.ToLower()))
            .OrderBy(w => w.Name)
            .ToListAsync();
    }
}