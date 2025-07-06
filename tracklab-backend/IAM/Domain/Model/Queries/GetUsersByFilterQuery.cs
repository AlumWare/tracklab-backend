namespace TrackLab.IAM.Domain.Model.Queries;

/// <summary>
/// Query to get users by filter criteria
/// </summary>
public record GetUsersByFilterQuery(long? UserId, long? TenantId, string? Role); 