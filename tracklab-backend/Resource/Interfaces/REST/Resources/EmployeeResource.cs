namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record EmployeeResource(
    long Id,
    string Dni,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Status,
    long PositionId
);
