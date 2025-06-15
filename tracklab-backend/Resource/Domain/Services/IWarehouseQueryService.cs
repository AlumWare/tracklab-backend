using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IWarehouseQueryService
{
    Task<IEnumerable<Warehouse>> ListAsync();
    Task<Warehouse?> FindByIdAsync(long id);
}