namespace TrackLab.IAM.Domain.Model.Queries;

/// <summary>
/// Query to get user by ID within current tenant
/// </summary>
public record GetUserByIdQuery(long UserId); 