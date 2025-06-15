using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class CreateEmployeeCommandFromResourceAssembler
{
    public static CreateEmployeeCommand ToCommandFromResource(CreateEmployeeResource resource)
    {
        return new CreateEmployeeCommand(
            resource.Dni,
            resource.Email,
            resource.FirstName,
            resource.LastName,
            resource.PhoneNumber,
            resource.PositionId
        );
    }
}