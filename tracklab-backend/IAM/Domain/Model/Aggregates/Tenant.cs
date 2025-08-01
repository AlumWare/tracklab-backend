using System.ComponentModel.DataAnnotations;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.IAM.Domain.Model.ValueObjects;

namespace TrackLab.IAM.Domain.Model.Aggregates;

/// <summary>
/// Tenant aggregate root for IAM context
/// This class represents a company/organization that uses the system
/// </summary>
public class Tenant
{
    public long Id { get; set; }
    
    [Required]
    [MaxLength(11)]
    public string Ruc { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string LegalName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? CommercialName { get; set; }
    
    [MaxLength(200)]
    public string? Address { get; set; }
    
    [MaxLength(50)]
    public string? City { get; set; }
    
    [MaxLength(50)]
    public string? Country { get; set; }
    
    public PhoneNumber? PhoneNumber { get; set; }
    
    public Email? Email { get; set; }
    
    [MaxLength(100)]
    public string? Website { get; set; }
    
    [Required]
    public bool Active { get; set; }
    
    [Required]
    public TenantType Type { get; set; } = TenantType.LOGISTIC;
    
    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
    public List<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Vehicle> Vehicles { get; set; } = new();
    public List<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Warehouse> Warehouses { get; set; } = new();
    public List<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Product> Products { get; set; } = new();
    public List<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Employee> Employees { get; set; } = new();
    public List<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Position> Positions { get; set; } = new();
    
    public Tenant()
    {
        Active = true;
        Type = TenantType.LOGISTIC;
    }
    
    public Tenant(string ruc, string legalName, string? commercialName, 
                  string? address, string? city, string? country, 
                  PhoneNumber? phone, Email? email, string? website, 
                  TenantType type = TenantType.LOGISTIC) : this()
    {
        Ruc = ruc;
        LegalName = legalName;
        Code = GenerateCodeFromRuc(ruc);
        CommercialName = commercialName;
        Address = address;
        City = city;
        Country = country;
        PhoneNumber = phone;
        Email = email;
        Website = website;
        Type = type;
    }
    
    /// <summary>
    /// Generate a unique code from RUC
    /// </summary>
    private string GenerateCodeFromRuc(string ruc)
    {
        // Simple code generation: TENANT_ + last 6 digits of RUC
        if (!string.IsNullOrEmpty(ruc) && ruc.Length >= 6)
        {
            return "TENANT_" + ruc.Substring(ruc.Length - 6);
        }
        return "TENANT_" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % 1000000;
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
    
    public string GetDisplayName()
    {
        return !string.IsNullOrEmpty(CommercialName) 
            ? CommercialName 
            : LegalName;
    }
    
    public string GetIdentifier() => Code;
    
    // Convenience methods for value objects
    public string? GetEmail() => Email?.Value;
    
    public string? GetPhone() => PhoneNumber?.Value;
    
    public bool IsLogistic() => Type == TenantType.LOGISTIC;
    public bool IsClient() => Type == TenantType.CLIENT;
    public bool IsProvider() => Type == TenantType.PROVIDER;
}
