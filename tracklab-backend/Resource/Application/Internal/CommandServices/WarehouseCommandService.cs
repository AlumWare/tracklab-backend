using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class WarehouseCommandService : IWarehouseCommandService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseCommandService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<Warehouse> Handle(CreateWarehouseCommand command)
    {
        var warehouse = new Warehouse(command);
        await _warehouseRepository.AddAsync(warehouse);
        return warehouse;
    }

    public async Task Handle(UpdateWarehouseInfoCommand command)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(command.WarehouseId);
        if (warehouse == null)
            throw new KeyNotFoundException($"Almacén con ID {command.WarehouseId} no encontrado.");

        warehouse.UpdateInfo(command);
        _warehouseRepository.Update(warehouse);
    }

    public async Task Handle(DeleteWarehouseCommand command)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(command.WarehouseId);
        if (warehouse == null)
            throw new KeyNotFoundException($"Almacén con ID {command.WarehouseId} no encontrado.");

        _warehouseRepository.Remove(warehouse);
    }
}