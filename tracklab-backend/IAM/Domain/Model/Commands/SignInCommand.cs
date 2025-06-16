namespace TrackLab.IAM.Domain.Model.Commands;

/// <summary>
/// Command for user authentication
/// </summary>
public record SignInCommand(
    string Username,
    string Password
);
