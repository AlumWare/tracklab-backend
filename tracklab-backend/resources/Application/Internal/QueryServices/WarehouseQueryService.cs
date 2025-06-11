using TrackLab.Domain.Model.Aggregates;
using TrackLab.Resources.Domain.Model.Queries;
using TrackLab.Resources.Domain.Repositories;
using TrackLab.Resources.Domain.Services;

namespace TrackLab.Resources.Application.Internal.QueryServices;

/// <summary>
/// Implementation of warehouse query service
/// </summary>
public class WarehouseQueryService(IWarehouseRepository warehouseRepository) : IWarehouseQueryService
{
    public async Task<Warehouse?> Handle(GetWarehouseByIdQuery query)
    {
        return await warehouseRepository.GetByIdAndTenantIdAsync(query.Id, query.TenantId);
    }

    public async Task<IEnumerable<Warehouse>> Handle(GetAllWarehousesQuery query)
    {
        return await warehouseRepository.GetByTenantIdAsync(query.TenantId);
    }

    public async Task<IEnumerable<Warehouse>> Handle(GetWarehousesByTypeQuery query)
    {
        return await warehouseRepository.GetByTenantIdAndTypeAsync(query.TenantId, query.Type);
    }
} 