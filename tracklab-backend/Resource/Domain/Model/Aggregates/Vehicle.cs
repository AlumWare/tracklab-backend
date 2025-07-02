using  Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using  Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;
using TenantId = TrackLab.Shared.Domain.ValueObjects.TenantId;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

public partial class Vehicle
{
    public long Id { get; private set; }
    public TenantId TenantId { get; private set; } = null!;
    public TrackLab.IAM.Domain.Model.Aggregates.Tenant Tenant { get; set; } = null!;
    public string LicensePlate { get; private set; } = null!;
    public decimal LoadCapacity { get; private set; }
    public int PaxCapacity { get; private set; }
    public EVehicleStatus Status { get; private set; }
    public Coordinates Location { get; private set; } = null!;
    public List<long> ImageAssetIds { get; private set; } = null!;
    
    public Vehicle() { }

    public Vehicle(CreateVehicleCommand command)
    {
        LicensePlate = command.LicensePlate;
        LoadCapacity = command.LoadCapacity;
        PaxCapacity = command.PaxCapacity;
        Location = command.Location;
        Status = EVehicleStatus.Available;
        ImageAssetIds = new List<long>();
    }

    public void UpdateStatus(UpdateVehicleStatusCommand command)
    {
        Status = command.NewStatus;
    }

    public void UpdateInfo(UpdateVehicleInfoCommand command)
    {
        LicensePlate = command.NewLicensePlate;
        LoadCapacity = command.NewLoadCapacity;
        PaxCapacity = command.NewPaxCapacity;
        Location = command.NewLocation;
    }

    public void Delete(DeleteVehicleCommand command)
    {
    }

    public void SetTenantId(TenantId tenantId)
    {
        TenantId = tenantId;
    }
}
