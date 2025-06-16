namespace TrackLab.IAM.Domain.Model.Commands;

/// <summary>
/// Command for creating a user within current tenant
/// </summary>
public record CreateUserCommand(
    string Username,
    string Password,
    string Email,
    string? FirstName,
    string? LastName,
    IEnumerable<string> Roles // List of role names
);
