namespace TrackLab.IAM.Interfaces.REST.Resources;

/// <summary>
/// Resource for sign in request payload
/// </summary>
public record SignInResource(
    string Username,
    string Password
); 