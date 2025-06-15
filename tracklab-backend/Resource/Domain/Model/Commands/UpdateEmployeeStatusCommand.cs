using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record UpdateEmployeeStatusCommand(
    long EmployeeId,
    EEmployeeStatus NewStatus
);