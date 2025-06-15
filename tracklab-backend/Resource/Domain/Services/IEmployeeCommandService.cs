using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IEmployeeCommandService
{
    Task<Employee> Handle(CreateEmployeeCommand command);
    Task Handle(UpdateEmployeeStatusCommand command);
    Task Handle(ChangeEmployeePositionCommand command);
    Task Handle(DeleteEmployeeCommand command);
}