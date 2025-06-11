using TrackLab.Domain.Model.Aggregates;
using TrackLab.Resources.Domain.Model.Commands;
using TrackLab.Resources.Domain.Repositories;
using TrackLab.Resources.Domain.Services;
using TrackLab.Shared.Domain.Repositories;

namespace TrackLab.Resources.Application.Internal.CommandServices;

/// <summary>
/// Implementation of warehouse command service
/// </summary>
public class WarehouseCommandService(IWarehouseRepository warehouseRepository, IUnitOfWork unitOfWork) 
    : IWarehouseCommandService
{
    public async Task<Warehouse> Handle(CreateWarehouseCommand command)
    {
        var warehouse = new Warehouse(command);
        
        await warehouseRepository.AddAsync(warehouse);
        await unitOfWork.CompleteAsync();
        
        return warehouse;
    }

    public async Task<Warehouse> Handle(UpdateWarehouseCommand command)
    {
        var warehouse = await warehouseRepository.GetByIdAndTenantIdAsync(command.Id, command.TenantId);
        
        if (warehouse == null)
            throw new InvalidOperationException($"Warehouse with id {command.Id} not found for tenant {command.TenantId}");
        
        warehouse.UpdateWarehouse(
            command.Name, 
            command.Type, 
            command.Street, 
            command.City, 
            command.State, 
            command.PostalCode, 
            command.Country,
            command.Latitude, 
            command.Longitude, 
            command.Phone, 
            command.Email);
        
        warehouseRepository.Update(warehouse);
        await unitOfWork.CompleteAsync();
        
        return warehouse;
    }

    public async Task Handle(DeleteWarehouseCommand command)
    {
        var warehouse = await warehouseRepository.GetByIdAndTenantIdAsync(command.Id, command.TenantId);
        
        if (warehouse == null)
            throw new InvalidOperationException($"Warehouse with id {command.Id} not found for tenant {command.TenantId}");
        
        warehouseRepository.Remove(warehouse);
        await unitOfWork.CompleteAsync();
    }
} 