using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public class Warehouse
{
    public long Id { get; private set; }
    public TenantId TenantId { get; private set; } = null!;

    public string Name { get; private set; } = null!;
    public EWarehouseType Type { get; private set; }

    public Coordinates Coordinates { get; private set; } = null!;
    public StreetAddress Address { get; private set; } = null!;


    public Warehouse() { }

    public Warehouse(CreateWarehouseCommand command)
    {
        Name = command.Name;
        Type = command.Type;
        Coordinates = new Coordinates(command.Latitude, command.Longitude);
        Address = new StreetAddress(command.Address);
        // El TenantId se establecerá desde el servicio usando el contexto actual
    }

    public void UpdateInfo(UpdateWarehouseInfoCommand command)
    {
        Name = command.Name;
        Coordinates = new Coordinates(command.Latitude, command.Longitude);
        Address = new StreetAddress(command.Address);
    }

    public void SetTenantId(TenantId tenantId)
    {
        TenantId = tenantId;
    }
}