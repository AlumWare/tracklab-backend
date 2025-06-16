namespace TrackLab.IAM.Interfaces.REST.Resources;

/// <summary>
/// Resource for authenticated user response
/// </summary>
public record AuthenticatedUserResource(
    long Id,
    string Username,
    string Token
); 