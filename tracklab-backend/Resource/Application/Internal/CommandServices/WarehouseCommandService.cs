using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class WarehouseCommandService : IWarehouseCommandService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public WarehouseCommandService(
        IWarehouseRepository warehouseRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<Warehouse> Handle(CreateWarehouseCommand command)
    {
        var warehouse = new Warehouse(command);
        
        // Establecer el tenant_id desde el contexto actual
        if (_tenantContext.HasTenant)
        {
            warehouse.SetTenantId(new TrackLab.Shared.Domain.ValueObjects.TenantId(_tenantContext.CurrentTenantId!.Value));
        }
        
        await _warehouseRepository.AddAsync(warehouse);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return warehouse;
    }

    public async Task Handle(UpdateWarehouseInfoCommand command)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(command.WarehouseId);
        if (warehouse == null)
            throw new KeyNotFoundException($"Almacén con ID {command.WarehouseId} no encontrado.");

        warehouse.UpdateInfo(command);
        _warehouseRepository.Update(warehouse);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task Handle(DeleteWarehouseCommand command)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(command.WarehouseId);
        if (warehouse == null)
            throw new KeyNotFoundException($"Almacén con ID {command.WarehouseId} no encontrado.");

        _warehouseRepository.Remove(warehouse);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }
}