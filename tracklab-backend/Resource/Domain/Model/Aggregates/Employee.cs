using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;
using SharedEmail = TrackLab.Shared.Domain.ValueObjects.Email;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public partial class Employee
{
    public long Id { get; private set; }
    public long TenantId { get; private set; }
    public TrackLab.IAM.Domain.Model.Aggregates.Tenant Tenant { get; set; } = null!;
    public Dni Dni { get; private set; } = null!;
    public SharedEmail Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public long PositionId { get; private set; }
    public Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Position Position { get; set; } = null!;
    public EEmployeeStatus Status { get; private set; }

    // Constructor requerido por EF Core
    public Employee() { }

    // Constructor de dominio
    public Employee(CreateEmployeeCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.FirstName))
            throw new ArgumentException("El nombre no puede estar vacío", nameof(command.FirstName));
        if (string.IsNullOrWhiteSpace(command.LastName))
            throw new ArgumentException("El apellido no puede estar vacío", nameof(command.LastName));
        if (string.IsNullOrWhiteSpace(command.PhoneNumber))
            throw new ArgumentException("El número de teléfono no puede estar vacío", nameof(command.PhoneNumber));
        if (command.PositionId <= 0)
            throw new ArgumentException("El ID de posición debe ser válido", nameof(command.PositionId));
        
        Dni = new Dni(command.Dni);
        Email = new SharedEmail(command.Email);
        FirstName = command.FirstName;
        LastName = command.LastName;
        PhoneNumber = command.PhoneNumber;
        PositionId = command.PositionId;
        Status = EEmployeeStatus.Available;
        // El TenantId se establecerá desde el servicio usando el contexto actual
    }

    public void UpdateStatus(UpdateEmployeeStatusCommand command)
    {
        Status = command.NewStatus;
    }

    public void ChangePosition(ChangeEmployeePositionCommand command)
    {
        PositionId = command.NewPositionId;
    }

    public void SetTenantId(long tenantId)
    {
        TenantId = tenantId;
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}".Trim();
    }

    public bool IsAvailable()
    {
        return Status == EEmployeeStatus.Available;
    }
}
