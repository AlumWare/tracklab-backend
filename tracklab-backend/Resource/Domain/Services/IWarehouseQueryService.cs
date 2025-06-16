using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IWarehouseQueryService
{
    Task<IEnumerable<Warehouse>> Handle(GetAllWarehousesQuery query);
    Task<Warehouse?> Handle(GetWarehouseByIdQuery query);
}