using Microsoft.EntityFrameworkCore;
using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Domain.Repositories;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Shared.Infrastructure.Multitenancy;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using TrackLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace TrackLab.IAM.Infrastructure.Persistence.EFC.Repositories;

/**
 * <summary>
 *     The user repository
 * </summary>
 * <remarks>
 *     This repository is used to manage users
 * </remarks>
 */
public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly ITenantContext _tenantContext;

    public UserRepository(AppDbContext context, ITenantContext tenantContext) : base(context)
    {
        _tenantContext = tenantContext;
    }

    /// <summary>
    /// Get queryable with tenant filter applied
    /// </summary>
    private IQueryable<User> GetTenantFilteredQuery()
    {
        var query = Context.Set<User>().AsQueryable();
        
        if (_tenantContext.HasTenant)
        {
            var currentTenantId = _tenantContext.CurrentTenantId!.Value;
            query = query.Where(u => u.TenantId.Value == currentTenantId);
        }
        
        return query;
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(u => u.Email.Value == email);
    }

    public async Task<User?> FindByUsernameAndTenantAsync(string username, TenantId tenantId)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == username && u.TenantId.Value == tenantId.Value);
    }

    public async Task<IEnumerable<User>> FindAllAsync()
    {
        return await GetTenantFilteredQuery()
            .OrderBy(u => u.Username)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> FindByRoleAsync(string roleName)
    {
        return await GetTenantFilteredQuery()
            .Where(u => u.Roles.Any(r => r.Name == roleName))
            .OrderBy(u => u.Username)
            .ToListAsync();
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await GetTenantFilteredQuery()
            .AnyAsync(u => u.Username == username);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await GetTenantFilteredQuery()
            .AnyAsync(u => u.Email.Value == email);
    }

    public async Task<User> SaveAsync(User user)
    {
        // Ensure tenant is set if creating new user
        if (user.Id == 0 && _tenantContext.HasTenant && user.TenantId.Value == 0)
        {
            user.TenantId = new TenantId(_tenantContext.CurrentTenantId!.Value);
        }

        if (user.Id == 0)
        {
            await AddAsync(user);
        }
        else
        {
            Update(user);
        }

        return user;
    }

    public async Task DeleteAsync(User user)
    {
        // Verify user belongs to current tenant for security
        if (_tenantContext.HasTenant && user.TenantId.Value != _tenantContext.CurrentTenantId!.Value)
        {
            throw new UnauthorizedAccessException("Cannot delete user from different tenant");
        }

        Remove(user);
        await Task.CompletedTask; // Avoid CS1998 warning
    }

    public async Task<(IEnumerable<User> Users, int TotalCount)> FindPaginatedAsync(int page, int size, string? searchTerm = null)
    {
        var query = GetTenantFilteredQuery();

        // Apply search filter if provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(u => 
                u.Username.ToLower().Contains(searchTerm) ||
                u.FirstName!.ToLower().Contains(searchTerm) ||
                u.LastName!.ToLower().Contains(searchTerm) ||
                u.Email.Value.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        
        var users = await query
            .OrderBy(u => u.Username)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (users, totalCount);
    }

    // Tenant-aware versions of base methods
    public new async Task<User?> FindByIdAsync(long id)
    {
        return await GetTenantFilteredQuery()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public new async Task<IEnumerable<User>> ListAsync()
    {
        return await FindAllAsync();
    }
}