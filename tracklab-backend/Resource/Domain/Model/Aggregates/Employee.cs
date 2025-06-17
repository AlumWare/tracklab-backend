using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;
using SharedEmail = TrackLab.Shared.Domain.ValueObjects.Email;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public class Employee
{
    public long Id { get; private set; }
    public TenantId TenantId { get; private set; } = null!;
    public Dni Dni { get; private set; } = null!;
    public SharedEmail Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public long PositionId { get; private set; }
    public EEmployeeStatus Status { get; private set; }

    // Constructor requerido por EF Core
    public Employee() { }

    // Constructor de dominio
    public Employee(CreateEmployeeCommand command)
    {
        Dni = new Dni(command.Dni);
        Email = new SharedEmail(command.Email);
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

    public void SetTenantId(TenantId tenantId)
    {
        TenantId = tenantId;
    }
}
