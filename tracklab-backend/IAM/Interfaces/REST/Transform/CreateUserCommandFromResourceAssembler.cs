using TrackLab.IAM.Domain.Model.Commands;
using TrackLab.IAM.Interfaces.REST.Resources;

namespace TrackLab.IAM.Interfaces.REST.Transform;

/// <summary>
/// Assembler to convert CreateUserResource to CreateUserCommand
/// </summary>
public static class CreateUserCommandFromResourceAssembler
{
    public static CreateUserCommand ToCommandFromResource(CreateUserResource resource)
    {
        return new CreateUserCommand(
            resource.Username,
            resource.Password,
            resource.Email,
            resource.FirstName,
            resource.LastName,
            resource.Roles
        );
    }
} 