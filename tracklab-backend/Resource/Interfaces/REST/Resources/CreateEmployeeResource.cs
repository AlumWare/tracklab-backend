namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record CreateEmployeeResource(
    string Dni,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    long PositionId
);