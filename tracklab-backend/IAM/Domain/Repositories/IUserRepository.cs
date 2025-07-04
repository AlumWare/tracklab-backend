using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Shared.Domain.Repositories;

namespace TrackLab.IAM.Domain.Repositories;

/// <summary>
/// Repository interface for User aggregate
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Find user by username within current tenant
    /// </summary>
    Task<User?> FindByUsernameAsync(string username);
    
    /// <summary>
    /// Find user by email within current tenant
    /// </summary>
    Task<User?> FindByEmailAsync(string email);
    
    /// <summary>
    /// Find user by username and tenant (cross-tenant for authentication)
    /// </summary>
    Task<User?> FindByUsernameAndTenantAsync(string username, long tenantId);
    
    /// <summary>
    /// Get all users within current tenant
    /// </summary>
    Task<IEnumerable<User>> FindAllAsync();
    
    /// <summary>
    /// Get users by role within current tenant
    /// </summary>
    Task<IEnumerable<User>> FindByRoleAsync(string roleName);
    
    /// <summary>
    /// Check if username exists in current tenant
    /// </summary>
    Task<bool> ExistsByUsernameAsync(string username);
    
    /// <summary>
    /// Check if email exists in current tenant
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email);
    
    /// <summary>
    /// Save user
    /// </summary>
    Task<User> SaveAsync(User user);
    
    /// <summary>
    /// Delete user
    /// </summary>
    Task DeleteAsync(User user);
    
    /// <summary>
    /// Get paginated users within current tenant
    /// </summary>
    Task<(IEnumerable<User> Users, int TotalCount)> FindPaginatedAsync(int page, int size, string? searchTerm = null);
}
