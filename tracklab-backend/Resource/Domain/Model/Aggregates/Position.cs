using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public partial class Position
{
    public long Id { get; private set; }
    public long TenantId { get; private set; }
    public TrackLab.IAM.Domain.Model.Aggregates.Tenant Tenant { get; set; } = null!;
    public List<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Employee> Employees { get; set; } = new();
    public string Name { get; private set; } = null!;
    
    public Position() { }

    // Constructor de dominio
    public Position(CreatePositionCommand command)
    {
        Name = command.Name;
        // El TenantId se establecerá desde el servicio usando el contexto actual
    }

    public void UpdateName(UpdatePositionNameCommand command)
    {
        Name = command.NewName;
    }

    public void SetTenantId(long tenantId)
    {
        TenantId = tenantId;
    }
}