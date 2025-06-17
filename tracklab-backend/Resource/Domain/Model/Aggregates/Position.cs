using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public class Position
{
    public long Id { get; private set; }
    public TenantId TenantId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    
    public Position() { }

    // Constructor de dominio
    public Position(CreatePositionCommand command)
    {
        Name = command.Name;
        TenantId = new TenantId(1); // temporal o configurable
    }

    public void UpdateName(UpdatePositionNameCommand command)
    {
        Name = command.NewName;
    }

    public void SetTenantId(TenantId tenantId)
    {
        TenantId = tenantId;
    }
}