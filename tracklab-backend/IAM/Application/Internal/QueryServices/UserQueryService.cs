using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Domain.Model.Queries;
using TrackLab.IAM.Domain.Repositories;
using TrackLab.IAM.Domain.Services;

namespace TrackLab.IAM.Application.Internal.QueryServices;

/// <summary>
/// Implementation of user query service
/// </summary>
public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository _userRepository;

    public UserQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        return await _userRepository.FindByIdAsync(query.UserId);
    }

    public async Task<User?> Handle(GetUserByUsernameQuery query)
    {
        return await _userRepository.FindByUsernameAsync(query.Username);
    }

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
    {
        return await _userRepository.FindAllAsync();
    }

    public async Task<IEnumerable<User>> Handle(GetUsersByFilterQuery query)
    {
        var users = await _userRepository.FindAllAsync();

        return users.Where(u => 
            (!query.UserId.HasValue || u.Id == query.UserId) &&
            (!query.TenantId.HasValue || u.TenantId == query.TenantId) &&
            (string.IsNullOrEmpty(query.Role) || u.HasRole(query.Role)));
    }
} 