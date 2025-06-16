using Microsoft.EntityFrameworkCore;

namespace TrackLab.IAM.Domain.Model.ValueObjects;

/// <summary>
/// Role value object with predefined system roles
/// </summary>
[Owned]
public record Role
{
    // Predefined roles
    public static readonly Role Admin = new("ROLE_ADMIN", "Administrator with full system access");
    public static readonly Role Operator = new("ROLE_OPERATOR", "Operator with operational access");
    public static readonly Role Supervisor = new("ROLE_SUPERVISOR", "Supervisor with oversight permissions");
    public static readonly Role Client = new("ROLE_CLIENT", "Client with limited access");
    public static readonly Role Provider = new("ROLE_PROVIDER", "Provider with supplier permissions");
    
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    
    public Role() { } // Required for EF
    
    private Role(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    /// <summary>
    /// Get all available roles
    /// </summary>
    public static Role[] GetAllRoles() => [Admin, Operator, Supervisor, Client, Provider];
    
    /// <summary>
    /// Create role from name (validates against predefined roles)
    /// </summary>
    public static Role FromName(string name)
    {
        return GetAllRoles().FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Invalid role: {name}. Must be one of: {string.Join(", ", GetAllRoles().Select(r => r.Name))}");
    }
    
    /// <summary>
    /// Check if role name is valid
    /// </summary>
    public static bool IsValidRole(string name)
    {
        return GetAllRoles().Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    
    // Convenience methods
    public bool IsAdmin() => this == Admin;
    public bool IsOperator() => this == Operator;
    public bool IsSupervisor() => this == Supervisor;
    public bool IsClient() => this == Client;
    public bool IsProvider() => this == Provider;
    
    public override string ToString() => Name;
    
    // Implicit conversion for convenience
    public static implicit operator string(Role role) => role.Name;
} 