using TrackLab.IAM.Domain.Model.Commands;
using TrackLab.IAM.Interfaces.REST.Resources;

namespace TrackLab.IAM.Interfaces.REST.Transform;

/// <summary>
/// Assembler to convert SignUpResource to SignUpCommand
/// </summary>
public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(
            resource.Ruc,
            resource.LegalName,
            resource.CommercialName,
            resource.Address,
            resource.City,
            resource.Country,
            resource.TenantPhone,
            resource.TenantEmail,
            resource.Website,
            resource.Username,
            resource.Password,
            resource.Email,
            resource.FirstName,
            resource.LastName,
            resource.TenantType
        );
    }
} 