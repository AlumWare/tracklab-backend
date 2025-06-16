namespace TrackLab.IAM.Interfaces.REST.Resources;

/// <summary>
/// Resource for sign up request payload
/// </summary>
public record SignUpResource(
    string Ruc,
    string LegalName,
    string? CommercialName,
    string? Address,
    string? City,
    string? Country,
    string? TenantPhone,
    string? TenantEmail,
    string? Website,
    string Username,
    string Password,
    string Email,
    string? FirstName,
    string? LastName,
    string? TenantType
);
