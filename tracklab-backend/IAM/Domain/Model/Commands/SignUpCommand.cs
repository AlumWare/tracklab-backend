namespace TrackLab.IAM.Domain.Model.Commands;

/// <summary>
/// Command for user and tenant registration
/// </summary>
public record SignUpCommand(
    // Tenant data
    string Ruc,
    string LegalName,
    string? CommercialName,
    string? Address,
    string? City,
    string? Country,
    string? TenantPhone,
    string? TenantEmail,
    string? Website,
    
    // User admin data
    string Username,
    string Password,
    string Email,
    string? FirstName,
    string? LastName,
    
    // Tenant type (optional - defaults to logistic)
    string? TenantType = "LOGISTIC" // LOGISTIC, CLIENT, PROVIDER
);
