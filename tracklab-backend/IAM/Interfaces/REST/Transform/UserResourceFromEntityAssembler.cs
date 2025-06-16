using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Interfaces.REST.Resources;

namespace TrackLab.IAM.Interfaces.REST.Transform;

/// <summary>
/// Assembler to convert User entity to UserResource
/// </summary>
public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User entity)
    {
        return new UserResource(
            entity.Id,
            entity.Username,
            entity.GetEmail() ?? string.Empty,
            entity.FirstName,
            entity.LastName,
            entity.GetFullName(),
            entity.Active,
            entity.TenantId.Value,
            entity.GetRoleNames()
        );
    }

    public static IEnumerable<UserResource> ToResourceFromEntity(IEnumerable<User> entities)
    {
        return entities.Select(ToResourceFromEntity);
    }
} 