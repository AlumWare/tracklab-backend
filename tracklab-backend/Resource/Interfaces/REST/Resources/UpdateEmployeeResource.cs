namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record UpdateEmployeeResource(
    string FirstName,
    string LastName,
    string PhoneNumber,
    long PositionId
);