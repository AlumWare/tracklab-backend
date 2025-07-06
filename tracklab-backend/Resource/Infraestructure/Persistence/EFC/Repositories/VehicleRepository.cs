using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Alumware.Tracklab.API.Resource.Infrastructure.Persistence.Repositories;

/**
 * <summary>
 *     The vehicle repository
 * </summary>
 * <remarks>
 *     This repository is used to manage vehicles with tenant awareness
 * </remarks>
 */
public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
{
    private readonly ITenantContext _tenantContext;

    public VehicleRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    /// <summary>
    /// Get queryable with tenant filter applied
    /// </summary>
    private IQueryable<Vehicle> GetTenantFilteredQuery()
    {
        var query = Context.Set<Vehicle>().AsQueryable();
        
        if (_tenantContext.HasTenant)
        {
            var currentTenantId = _tenantContext.CurrentTenantId!.Value;
            query = query.Where(v => v.TenantId == currentTenantId);
        }
        
        return query;
    }

    public new async Task<IEnumerable<Vehicle>> ListAsync()
    {
        return await GetTenantFilteredQuery()
            .OrderBy(v => v.LicensePlate)
            .ToListAsync();
    }

    public new async Task<Vehicle?> FindByIdAsync(long id)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public Vehicle SaveAsync(Vehicle vehicle)
    {
        // Ensure tenant is set if creating new vehicle
        if (vehicle.Id == 0 && _tenantContext.HasTenant && vehicle.TenantId == 0)
        {
            vehicle.SetTenantId(_tenantContext.CurrentTenantId!.Value);
        }

        if (vehicle.Id == 0)
        {
            AddAsync(vehicle).Wait();
        }
        else
        {
            Update(vehicle);
        }

        return vehicle;
    }

    public void DeleteAsync(Vehicle vehicle)
    {
        // Verify vehicle belongs to current tenant for security
        if (_tenantContext.HasTenant && vehicle.TenantId != _tenantContext.CurrentTenantId!.Value)
        {
            throw new UnauthorizedAccessException("Cannot delete vehicle from different tenant");
        }

        Remove(vehicle);
    }

    public async Task<(IEnumerable<Vehicle> Vehicles, int TotalCount)> FindPaginatedAsync(int page, int size, string? searchTerm = null)
    {
        var query = GetTenantFilteredQuery();

        // Apply search filter if provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(v => 
                v.LicensePlate.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        
        var vehicles = await query
            .OrderBy(v => v.LicensePlate)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (vehicles, totalCount);
    }

    public async Task<IEnumerable<Vehicle>> FindByStatusAsync(string status)
    {
        if (Enum.TryParse<Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects.EVehicleStatus>(status, out var statusEnum))
        {
            return await GetTenantFilteredQuery()
                .Where(v => v.Status == statusEnum)
                .OrderBy(v => v.LicensePlate)
                .ToListAsync();
        }
        
        return Enumerable.Empty<Vehicle>();
    }

    public async Task<IEnumerable<Vehicle>> FindByLicensePlateAsync(string licensePlate)
    {
        return await GetTenantFilteredQuery()
            .Where(v => v.LicensePlate.ToLower().Contains(licensePlate.ToLower()))
            .OrderBy(v => v.LicensePlate)
            .ToListAsync();
    }

    public async Task<Vehicle?> FindByLicensePlateExactAsync(string licensePlate)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(v => v.LicensePlate.ToLower() == licensePlate.ToLower());
    }
}