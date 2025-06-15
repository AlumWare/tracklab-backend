namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record CreateEmployeeCommand(
    string Dni,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    long PositionId
);