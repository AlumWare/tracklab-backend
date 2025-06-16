namespace TrackLab.IAM.Domain.Model.Queries;

/// <summary>
/// Query to get user by username (cross-tenant for authentication)
/// </summary>
public record GetUserByUsernameQuery(string Username); 