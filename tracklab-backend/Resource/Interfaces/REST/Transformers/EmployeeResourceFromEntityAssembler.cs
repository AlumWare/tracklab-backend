using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;
namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class EmployeeResourceFromEntityAssembler
{
    public static EmployeeResource ToResourceFromEntity(Employee employee)
    {
        return new EmployeeResource(
            employee.Id,
            employee.Dni.Value,
            employee.Email.Value,
            employee.FirstName,
            employee.LastName,
            employee.PhoneNumber,
            employee.Status.ToString(),
            employee.PositionId
        );
    }
}