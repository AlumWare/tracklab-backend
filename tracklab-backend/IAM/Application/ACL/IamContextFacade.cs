using TrackLab.IAM.Domain.Services;
using TrackLab.IAM.Domain.Model.Queries;
using TrackLab.IAM.Interfaces.ACL;

namespace TrackLab.IAM.Application.ACL;

public class IamContextFacade : IIamContextFacade
{
    private readonly IUserQueryService _userQueryService;

    public IamContextFacade(IUserQueryService userQueryService)
    {
        _userQueryService = userQueryService;
    }

    public async Task<IEnumerable<string>> GetUserEmailsAsync(long? userId = null, long? tenantId = null, string? role = null)
    {
        var query = new GetUsersByFilterQuery(userId, tenantId, role);
        var users = await _userQueryService.Handle(query);
        return users.Select(u => u.Email.Value).Distinct();
    }
}
