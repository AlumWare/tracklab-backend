using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IEmployeeQueryService
{
    Task<IEnumerable<Employee>> Handle(GetAllEmployeesQuery query);
    Task<Employee?> Handle(GetEmployeeByIdQuery query);
}