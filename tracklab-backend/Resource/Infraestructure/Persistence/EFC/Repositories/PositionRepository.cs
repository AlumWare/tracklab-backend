using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Alumware.Tracklab.API.Resource.Infrastructure.Persistence.Repositories;

/**
 * <summary>
 *     The position repository
 * </summary>
 * <remarks>
 *     This repository is used to manage positions with tenant awareness
 * </remarks>
 */
public class PositionRepository : BaseRepository<Position>, IPositionRepository
{
    private readonly ITenantContext _tenantContext;

    public PositionRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    /// <summary>
    /// Get queryable with tenant filter applied
    /// </summary>
    private IQueryable<Position> GetTenantFilteredQuery()
    {
        var query = Context.Set<Position>().AsQueryable();
        
        if (_tenantContext.HasTenant)
        {
            var currentTenantId = _tenantContext.CurrentTenantId!.Value;
            query = query.Where(p => p.TenantId.Value == currentTenantId);
        }
        
        return query;
    }

    public new async Task<IEnumerable<Position>> ListAsync()
    {
        return await GetTenantFilteredQuery()
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public new async Task<Position?> FindByIdAsync(long id)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Position SaveAsync(Position position)
    {
        // Ensure tenant is set if creating new position
        if (position.Id == 0 && _tenantContext.HasTenant && position.TenantId.Value == 0)
        {
            position.SetTenantId(new TenantId(_tenantContext.CurrentTenantId!.Value));
        }

        if (position.Id == 0)
        {
            AddAsync(position).Wait();
        }
        else
        {
            Update(position);
        }

        return position;
    }

    public void DeleteAsync(Position position)
    {
        // Verify position belongs to current tenant for security
        if (_tenantContext.HasTenant && position.TenantId.Value != _tenantContext.CurrentTenantId!.Value)
        {
            throw new UnauthorizedAccessException("Cannot delete position from different tenant");
        }

        Remove(position);
    }

    public async Task<(IEnumerable<Position> Positions, int TotalCount)> FindPaginatedAsync(int page, int size, string? searchTerm = null)
    {
        var query = GetTenantFilteredQuery();

        // Apply search filter if provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(p => 
                p.Name.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        
        var positions = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (positions, totalCount);
    }

    public async Task<IEnumerable<Position>> FindByNameAsync(string name)
    {
        return await GetTenantFilteredQuery()
            .Where(p => p.Name.ToLower().Contains(name.ToLower()))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}