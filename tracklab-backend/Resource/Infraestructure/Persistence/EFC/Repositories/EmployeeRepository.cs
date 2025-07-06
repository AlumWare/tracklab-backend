using Microsoft.EntityFrameworkCore;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Alumware.Tracklab.API.Resource.Infrastructure.Persistence.Repositories;

/**
 * <summary>
 *     The employee repository
 * </summary>
 * <remarks>
 *     This repository is used to manage employees with tenant awareness
 * </remarks>
 */
public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    private readonly ITenantContext _tenantContext;

    public EmployeeRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    /// <summary>
    /// Get queryable with tenant filter applied
    /// </summary>
    private IQueryable<Employee> GetTenantFilteredQuery()
    {
        var query = Context.Set<Employee>().AsQueryable();
        
        if (_tenantContext.HasTenant)
        {
            var currentTenantId = _tenantContext.CurrentTenantId!.Value;
            query = query.Where(e => e.TenantId == currentTenantId);
        }
        
        return query;
    }

    public new async Task<IEnumerable<Employee>> ListAsync()
    {
        return await GetTenantFilteredQuery()
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync();
    }

    public new async Task<Employee?> FindByIdAsync(long id)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public Employee SaveAsync(Employee employee)
    {
        // Ensure tenant is set if creating new employee
        if (employee.Id == 0 && _tenantContext.HasTenant && employee.TenantId == 0)
        {
            employee.SetTenantId(_tenantContext.CurrentTenantId!.Value);
        }

        if (employee.Id == 0)
        {
            AddAsync(employee).Wait();
        }
        else
        {
            Update(employee);
        }

        return employee;
    }

    public void DeleteAsync(Employee employee)
    {
        // Verify employee belongs to current tenant for security
        if (_tenantContext.HasTenant && employee.TenantId != _tenantContext.CurrentTenantId!.Value)
        {
            throw new UnauthorizedAccessException("Cannot delete employee from different tenant");
        }

        Remove(employee);
    }

    public async Task<(IEnumerable<Employee> Employees, int TotalCount)> FindPaginatedAsync(int page, int size, string? searchTerm = null)
    {
        var query = GetTenantFilteredQuery();

        // Apply search filter if provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(e => 
                e.FirstName.ToLower().Contains(searchTerm) ||
                e.LastName.ToLower().Contains(searchTerm) ||
                e.Email.Value.ToLower().Contains(searchTerm) ||
                e.Dni.Value.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        
        var employees = await query
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (employees, totalCount);
    }

    public async Task<IEnumerable<Employee>> FindByStatusAsync(string status)
    {
        if (Enum.TryParse<Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects.EEmployeeStatus>(status, out var statusEnum))
        {
            return await GetTenantFilteredQuery()
                .Where(e => e.Status == statusEnum)
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToListAsync();
        }
        
        return Enumerable.Empty<Employee>();
    }

    public async Task<IEnumerable<Employee>> FindByPositionAsync(long positionId)
    {
        return await GetTenantFilteredQuery()
            .Where(e => e.PositionId == positionId)
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync();
    }
}