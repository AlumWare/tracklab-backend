using TrackLab.IAM.Domain.Model.Commands;
using TrackLab.IAM.Interfaces.REST.Resources;

namespace TrackLab.IAM.Interfaces.REST.Transform;

/// <summary>
/// Assembler to convert SignInResource to SignInCommand
/// </summary>
public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(
            resource.Username,
            resource.Password
        );
    }
} 