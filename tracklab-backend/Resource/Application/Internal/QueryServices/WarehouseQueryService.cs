using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
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

    public async Task<IEnumerable<Warehouse>> ListAsync()
    {
        return await _warehouseRepository.ListAsync();
    }

    public async Task<Warehouse?> FindByIdAsync(long id)
    {
        return await _warehouseRepository.FindByIdAsync(id);
    }
}