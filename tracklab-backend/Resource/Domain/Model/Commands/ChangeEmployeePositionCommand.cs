namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record ChangeEmployeePositionCommand(
    long EmployeeId,
    long NewPositionId
);