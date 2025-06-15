using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IEmployeeQueryService
{
    Task<IEnumerable<Employee>> ListAsync();
    Task<Employee?> FindByIdAsync(long id);
}