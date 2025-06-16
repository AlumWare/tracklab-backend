using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Interfaces.REST.Resources;

namespace TrackLab.IAM.Interfaces.REST.Transform;

/// <summary>
/// Assembler to convert User entity and token to AuthenticatedUserResource
/// </summary>
public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token)
    {
        return new AuthenticatedUserResource(
            user.Id,
            user.Username,
            token
        );
    }
}
