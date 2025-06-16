using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
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

    public async Task<IEnumerable<Employee>> Handle(GetAllEmployeesQuery query)
    {
        var employees = await _employeeRepository.ListAsync();
        
        // Aplicar filtros si están especificados
        if (!string.IsNullOrEmpty(query.Status))
        {
            if (Enum.TryParse<EEmployeeStatus>(query.Status, out var status))
            {
                employees = employees.Where(e => e.Status == status);
            }
        }
        
        // Nota: Para filtrar por posición necesitaríamos hacer un join con la tabla de posiciones
        // Por ahora solo aplicamos paginación
        
        // Aplicar paginación si está especificada
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
        {
            employees = employees
                .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                .Take(query.PageSize.Value);
        }
        
        return employees;
    }

    public async Task<Employee?> Handle(GetEmployeeByIdQuery query)
    {
        return await _employeeRepository.FindByIdAsync(query.Id);
    }
}