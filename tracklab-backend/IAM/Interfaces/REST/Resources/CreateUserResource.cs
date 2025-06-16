namespace TrackLab.IAM.Interfaces.REST.Resources;

/// <summary>
/// Resource for create user request payload
/// </summary>
public record CreateUserResource(
    string Username,
    string Password,
    string Email,
    string? FirstName,
    string? LastName,
    IEnumerable<string> Roles
); 