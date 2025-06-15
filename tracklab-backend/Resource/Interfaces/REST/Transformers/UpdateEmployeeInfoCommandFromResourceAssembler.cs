using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class UpdateEmployeeInfoCommandFromResourceAssembler
{
    public static ChangeEmployeePositionCommand ToPositionCommand(long employeeId, UpdateEmployeeResource resource)
    {
        return new ChangeEmployeePositionCommand(employeeId, resource.PositionId);
    }

    public static UpdateEmployeeStatusCommand ToStatusCommand(long employeeId, string newStatus)
    {
        Enum.TryParse(newStatus, out EEmployeeStatus statusEnum);
        return new UpdateEmployeeStatusCommand(employeeId, statusEnum);
    }
}