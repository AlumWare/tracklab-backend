using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Domain.Model.Queries;

namespace TrackLab.IAM.Domain.Services;

/// <summary>
/// Interface for user query service
/// </summary>
public interface IUserQueryService
{
    /// <summary>
    /// Handle get user by ID query
    /// </summary>
    Task<User?> Handle(GetUserByIdQuery query);
    
    /// <summary>
    /// Handle get user by username query
    /// </summary>
    Task<User?> Handle(GetUserByUsernameQuery query);
    
    /// <summary>
    /// Handle get all users query
    /// </summary>
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query);
}
