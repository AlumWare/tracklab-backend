using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public class Employee
{
    public long Id { get; private set; }
    public TenantId TenantId { get; private set; }
    public Dni Dni { get; private set; }
    public Email Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public long PositionId { get; private set; }
    public EEmployeeStatus Status { get; private set; }

    // Constructor requerido por EF Core
    public Employee() { }

    // Constructor de dominio
    public Employee(CreateEmployeeCommand command)
    {
        Dni = new Dni(command.Dni);
        Email = new Email(command.Email);
        FirstName = command.FirstName;
        LastName = command.LastName;
        PhoneNumber = command.PhoneNumber;
        PositionId = command.PositionId;
        Status = EEmployeeStatus.Available;
        TenantId = new TenantId(1);
    }

    public void UpdateStatus(UpdateEmployeeStatusCommand command)
    {
        Status = command.NewStatus;
    }

    public void ChangePosition(ChangeEmployeePositionCommand command)
    {
        PositionId = command.NewPositionId;
    }
}
