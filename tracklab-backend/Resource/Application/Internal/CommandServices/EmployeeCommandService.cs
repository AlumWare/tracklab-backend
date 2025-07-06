using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class EmployeeCommandService : IEmployeeCommandService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPositionRepository _positionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public EmployeeCommandService(
        IEmployeeRepository employeeRepository,
        IPositionRepository positionRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _employeeRepository = employeeRepository;
        _positionRepository = positionRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<Employee> Handle(CreateEmployeeCommand command)
    {
        // Validar que la posición existe
        var position = await _positionRepository.FindByIdAsync(command.PositionId);
        if (position == null)
            throw new ArgumentException($"La posición con ID {command.PositionId} no existe.");
        
        var employee = new Employee(command);
        
        // Establecer el tenant_id desde el contexto actual
        if (_tenantContext.HasTenant)
        {
            employee.SetTenantId(_tenantContext.CurrentTenantId!.Value);
        }
        
        await _employeeRepository.AddAsync(employee);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        
        return employee;
    }

    public async Task Handle(UpdateEmployeeStatusCommand command)
    {
        var employee = await _employeeRepository.FindByIdAsync(command.EmployeeId);
        if (employee == null)
            throw new KeyNotFoundException($"Empleado {command.EmployeeId} no encontrado.");

        employee.UpdateStatus(command);
        _employeeRepository.Update(employee);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task Handle(ChangeEmployeePositionCommand command)
    {
        var employee = await _employeeRepository.FindByIdAsync(command.EmployeeId);
        if (employee == null)
            throw new KeyNotFoundException($"Empleado {command.EmployeeId} no encontrado.");

        employee.ChangePosition(command);
        _employeeRepository.Update(employee);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task Handle(DeleteEmployeeCommand command)
    {
        var employee = await _employeeRepository.FindByIdAsync(command.EmployeeId);
        if (employee == null)
            throw new KeyNotFoundException($"Empleado {command.EmployeeId} no encontrado.");

        _employeeRepository.Remove(employee);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }
}