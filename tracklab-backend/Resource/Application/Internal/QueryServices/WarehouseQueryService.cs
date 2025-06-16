using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;

namespace Alumware.Tracklab.API.Resource.Application.Internal.QueryServices;

public class WarehouseQueryService : IWarehouseQueryService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseQueryService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<IEnumerable<Warehouse>> Handle(GetAllWarehousesQuery query)
    {
        var warehouses = await _warehouseRepository.ListAsync();
        
        // Aplicar filtros si están especificados
        if (!string.IsNullOrEmpty(query.Location))
        {
            warehouses = warehouses.Where(w => 
                w.Address.ToString().Contains(query.Location, StringComparison.OrdinalIgnoreCase) ||
                w.Coordinates.ToString().Contains(query.Location, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrEmpty(query.Type))
        {
            if (Enum.TryParse<EWarehouseType>(query.Type, out var type))
            {
                warehouses = warehouses.Where(w => w.Type == type);
            }
        }
        
        // Nota: El filtro IsActive no existe en el modelo actual de Warehouse
        // Solo aplicamos paginación
        
        // Aplicar paginación si está especificada
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
        {
            warehouses = warehouses
                .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                .Take(query.PageSize.Value);
        }
        
        return warehouses;
    }

    public async Task<Warehouse?> Handle(GetWarehouseByIdQuery query)
    {
        return await _warehouseRepository.FindByIdAsync(query.Id);
    }
}