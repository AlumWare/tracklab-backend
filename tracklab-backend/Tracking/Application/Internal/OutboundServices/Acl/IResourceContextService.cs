using Alumware.Tracklab.API.Resource.Interfaces.ACL;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.OutboundServices.ACL;

/// <summary>
/// Anti-corruption layer for ResourceContext services
/// </summary>
public interface IResourceContextService
{
    /// <summary>
    /// Validates if a warehouse exists and is active
    /// </summary>
    Task<bool> ValidateWarehouseExistsAsync(long warehouseId);
    
    /// <summary>
    /// Gets basic warehouse information
    /// </summary>
    Task<Alumware.Tracklab.API.Resource.Interfaces.ACL.WarehouseInfo?> GetWarehouseInfoAsync(long warehouseId);
    
    /// <summary>
    /// Validates if a vehicle exists and is available
    /// </summary>
    Task<bool> ValidateVehicleExistsAsync(long vehicleId);
    
    /// <summary>
    /// Gets basic vehicle information
    /// </summary>
    Task<Alumware.Tracklab.API.Resource.Interfaces.ACL.VehicleInfo?> GetVehicleInfoAsync(long vehicleId);
    
    /// <summary>
    /// Validates if an employee exists and is active
    /// </summary>
    Task<bool> ValidateEmployeeExistsAsync(long employeeId);
    
    /// <summary>
    /// Gets basic employee information
    /// </summary>
    Task<Alumware.Tracklab.API.Resource.Interfaces.ACL.EmployeeInfo?> GetEmployeeInfoAsync(long employeeId);

    /// <summary>
    /// Gets warehouse information including type
    /// </summary>
    Task<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Warehouse?> GetWarehouseAsync(long warehouseId);
} 