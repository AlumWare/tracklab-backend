using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class EmployeeCommandService : IEmployeeCommandService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeCommandService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee> Handle(CreateEmployeeCommand command)
    {
        var employee = new Employee(command);
        await _employeeRepository.AddAsync(employee);
        return employee;
    }

    public async Task Handle(UpdateEmployeeStatusCommand command)
    {
        var employee = await _employeeRepository.FindByIdAsync(command.EmployeeId);
        if (employee == null)
            throw new KeyNotFoundException($"Empleado {command.EmployeeId} no encontrado.");

        employee.UpdateStatus(command);
        _employeeRepository.Update(employee);
    }

    public async Task Handle(ChangeEmployeePositionCommand command)
    {
        var employee = await _employeeRepository.FindByIdAsync(command.EmployeeId);
        if (employee == null)
            throw new KeyNotFoundException($"Empleado {command.EmployeeId} no encontrado.");

        employee.ChangePosition(command);
        _employeeRepository.Update(employee);
    }

    public async Task Handle(DeleteEmployeeCommand command)
    {
        var employee = await _employeeRepository.FindByIdAsync(command.EmployeeId);
        if (employee == null)
            throw new KeyNotFoundException($"Empleado {command.EmployeeId} no encontrado.");

        _employeeRepository.Remove(employee);
    }
}