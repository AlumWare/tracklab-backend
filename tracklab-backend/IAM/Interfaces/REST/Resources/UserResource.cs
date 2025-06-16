namespace TrackLab.IAM.Interfaces.REST.Resources;

/// <summary>
/// Resource for user response
/// </summary>
public record UserResource(
    long Id,
    string Username,
    string Email,
    string? FirstName,
    string? LastName,
    string FullName,
    bool Active,
    long TenantId,
    IEnumerable<string> Roles
); 