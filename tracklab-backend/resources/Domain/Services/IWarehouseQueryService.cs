using TrackLab.Domain.Model.Aggregates;
using TrackLab.Resources.Domain.Model.Queries;

namespace TrackLab.Resources.Domain.Services;

/// <summary>
/// Interface for warehouse query service
/// </summary>
public interface IWarehouseQueryService
{
    /// <summary>
    /// Gets a warehouse by id
    /// </summary>
    /// <param name="query">Get warehouse by id query</param>
    /// <returns>Warehouse or null if not found</returns>
    Task<Warehouse?> Handle(GetWarehouseByIdQuery query);
    
    /// <summary>
    /// Gets all warehouses for a tenant
    /// </summary>
    /// <param name="query">Get all warehouses query</param>
    /// <returns>List of warehouses</returns>
    Task<IEnumerable<Warehouse>> Handle(GetAllWarehousesQuery query);
    
    /// <summary>
    /// Gets warehouses by type for a tenant
    /// </summary>
    /// <param name="query">Get warehouses by type query</param>
    /// <returns>List of warehouses</returns>
    Task<IEnumerable<Warehouse>> Handle(GetWarehousesByTypeQuery query);
}
