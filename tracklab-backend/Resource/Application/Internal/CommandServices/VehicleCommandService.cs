using Alumware.Tracklab.API.Resource.Domain.Services;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class VehicleCommandService : IVehicleCommandService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleCommandService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Vehicle> Handle(CreateVehicleCommand command)
    {
        var vehicle = new Vehicle(command);
        await _vehicleRepository.AddAsync(vehicle);
        return vehicle;
    }

    public async Task Handle(UpdateVehicleStatusCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        vehicle.UpdateStatus(command);
        _vehicleRepository.Update(vehicle);
    }

    public async Task Handle(UpdateVehicleInfoCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        vehicle.UpdateInfo(command);
        _vehicleRepository.Update(vehicle);
    }

    public async Task Handle(DeleteVehicleCommand command)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehículo con ID {command.VehicleId} no encontrado.");

        _vehicleRepository.Remove(vehicle);
    }
}