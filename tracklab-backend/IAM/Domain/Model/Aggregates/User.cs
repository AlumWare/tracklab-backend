using System.ComponentModel.DataAnnotations;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.IAM.Domain.Model.ValueObjects;

namespace TrackLab.IAM.Domain.Model.Aggregates;

/// <summary>
/// User aggregate root for IAM context
/// This class represents the aggregate root for the User entity with multitenancy support.
/// </summary>
public class User
{
    public long Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(120)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public Email Email { get; set; } = default!;
    
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [MaxLength(50)]
    public string? LastName { get; set; }
    
    [Required]
    public bool Active { get; set; }
    
    public long TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    
    // Roles as a collection of value objects
    private readonly List<Role> _roles = [];
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
    
    // Propiedad para EF Core (no expuesta públicamente)
    internal List<Role> RolesInternal 
    { 
        get => _roles; 
        set 
        { 
            _roles.Clear();
            if (value != null)
                _roles.AddRange(value);
        } 
    }
    
    public User()
    {
        Active = true;
    }
    
    public User(string username, string passwordHash, Email email, 
                string? firstName, string? lastName, TenantId tenantId) : this()
    {
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        TenantId = tenantId;
    }
    
    public User(string username, string passwordHash, Email email, 
                string? firstName, string? lastName, TenantId tenantId, 
                IEnumerable<Role> roles) : this(username, passwordHash, email, firstName, lastName, tenantId)
    {
        AddRoles(roles);
    }
    
    public void Activate()
    {
        Active = true;
    }
    
    public void Deactivate()
    {
        Active = false;
    }
    
    public bool IsActive() => Active;
    
    public string GetFullName()
    {
        return $"{FirstName} {LastName}".Trim();
    }
    
    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }
    
    // Convenience method for email
    public string? GetEmail() => Email?.Value;
    
    public void SetEmail(string? emailValue)
    {
        Email = emailValue != null ? new Email(emailValue) : default!;
    }
    
    /// <summary>
    /// Add a role to the user
    /// </summary>
    public User AddRole(Role role)
    {
        if (!_roles.Contains(role))
        {
            _roles.Add(role);
        }
        return this;
    }
    
    /// <summary>
    /// Add a role by name
    /// </summary>
    public User AddRole(string roleName)
    {
        var role = Role.FromName(roleName);
        return AddRole(role);
    }
    
    /// <summary>
    /// Add multiple roles to the user
    /// </summary>
    public User AddRoles(IEnumerable<Role> roles)
    {
        foreach (var role in roles)
        {
            AddRole(role);
        }
        return this;
    }
    
    /// <summary>
    /// Remove a role from the user
    /// </summary>
    public User RemoveRole(Role role)
    {
        _roles.Remove(role);
        return this;
    }
    
    /// <summary>
    /// Remove a role by name
    /// </summary>
    public User RemoveRole(string roleName)
    {
        var role = Role.FromName(roleName);
        return RemoveRole(role);
    }
    
    /// <summary>
    /// Check if user has a specific role
    /// </summary>
    public bool HasRole(Role role)
    {
        return _roles.Contains(role);
    }
    
    /// <summary>
    /// Check if user has a specific role by name
    /// </summary>
    public bool HasRole(string roleName)
    {
        return _roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }
    
    /// <summary>
    /// Check if user is admin
    /// </summary>
    public bool IsAdmin() => HasRole(Role.Admin);
    
    /// <summary>
    /// Check if user is operator
    /// </summary>
    public bool IsOperator() => HasRole(Role.Operator);
    
    /// <summary>
    /// Check if user is supervisor
    /// </summary>
    public bool IsSupervisor() => HasRole(Role.Supervisor);
    
    /// <summary>
    /// Check if user is client
    /// </summary>
    public bool IsClient() => HasRole(Role.Client);
    
    /// <summary>
    /// Check if user is provider
    /// </summary>
    public bool IsProvider() => HasRole(Role.Provider);
    
    /// <summary>
    /// Get all role names for this user
    /// </summary>
    public IEnumerable<string> GetRoleNames()
    {
        return _roles.Select(r => r.Name);
    }
    
    /// <summary>
    /// Clear all roles
    /// </summary>
    public void ClearRoles()
    {
        _roles.Clear();
    }
}
