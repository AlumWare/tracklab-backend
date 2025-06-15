using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;

namespace Alumware.Tracklab.API.Resource.Application.Internal.QueryServices;

public class EmployeeQueryService : IEmployeeQueryService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeQueryService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<Employee>> ListAsync()
    {
        return await _employeeRepository.ListAsync();
    }

    public async Task<Employee?> FindByIdAsync(long id)
    {
        return await _employeeRepository.FindByIdAsync(id);
    }
}