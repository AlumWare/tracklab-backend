using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class VehicleCommandService : IVehicleCommandService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public VehicleCommandService(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<Vehicle> Handle(CreateVehicleCommand command)
    {
        var vehicle = new Vehicle(command);
        
        // Establecer el tenant_id desde el contexto actual
        if (_tenantContext.HasTenant)
        {
            vehicle.SetTenantId(new TrackLab.Shared.Domain.ValueObjects.TenantId(_tenantContext.CurrentTenantId!.Value));
        }
        
        await _vehicleRepository.AddAsync(vehicle);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return vehicle;
    }

    public async Task Handle(UpdateVehicleStatusCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        vehicle.UpdateStatus(command);
        _vehicleRepository.Update(vehicle);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task Handle(UpdateVehicleInfoCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        vehicle.UpdateInfo(command);
        _vehicleRepository.Update(vehicle);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task Handle(DeleteVehicleCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        _vehicleRepository.Remove(vehicle);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }
}