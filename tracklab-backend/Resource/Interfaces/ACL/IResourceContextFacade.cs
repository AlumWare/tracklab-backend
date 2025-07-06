namespace Alumware.Tracklab.API.Resource.Interfaces.ACL;

/// <summary>
/// Facade interface exposed by ResourceContext for other contexts integration
/// </summary>
public interface IResourceContextFacade
{
    /// <summary>
    /// Validates if a product exists and is available
    /// </summary>
    Task<bool> ValidateProductExistsAsync(long productId);
    
    /// <summary>
    /// Gets basic product information
    /// </summary>
    Task<ProductInfo?> GetProductInfoAsync(long productId);
    
    /// <summary>
    /// Gets products information for multiple IDs
    /// </summary>
    Task<IEnumerable<ProductInfo>> GetProductsInfoAsync(IEnumerable<long> productIds);
    
    /// <summary>
    /// Validates if a warehouse exists and is active
    /// </summary>
    Task<bool> ValidateWarehouseExistsAsync(long warehouseId);
    
    /// <summary>
    /// Gets basic warehouse information
    /// </summary>
    Task<WarehouseInfo?> GetWarehouseInfoAsync(long warehouseId);
    
    /// <summary>
    /// Gets complete warehouse information including domain model
    /// </summary>
    Task<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Warehouse?> GetWarehouseAsync(long warehouseId);
    
    /// <summary>
    /// Validates if a vehicle exists and is available
    /// </summary>
    Task<bool> ValidateVehicleExistsAsync(long vehicleId);
    
    /// <summary>
    /// Gets basic vehicle information
    /// </summary>
    Task<VehicleInfo?> GetVehicleInfoAsync(long vehicleId);
    
    /// <summary>
    /// Validates if an employee exists and is active
    /// </summary>
    Task<bool> ValidateEmployeeExistsAsync(long employeeId);
    
    /// <summary>
    /// Gets basic employee information
    /// </summary>
    Task<EmployeeInfo?> GetEmployeeInfoAsync(long employeeId);
    
    /// <summary>
    /// Validates if a position exists
    /// </summary>
    Task<bool> ValidatePositionExistsAsync(long positionId);
}

/// <summary>
/// Basic product information for external contexts
/// </summary>
public record ProductInfo(
    long Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string Category,
    int Stock
);

/// <summary>
/// Basic warehouse information for external contexts
/// </summary>
public record WarehouseInfo(
    long Id,
    string Name,
    string Address,
    string Type,
    double Latitude,
    double Longitude
);

/// <summary>
/// Basic vehicle information for external contexts
/// </summary>
public record VehicleInfo(
    long Id,
    string LicensePlate,
    decimal LoadCapacity,
    int PaxCapacity,
    string Status,
    decimal Tonnage,
    double Latitude,
    double Longitude
);

/// <summary>
/// Basic employee information for external contexts
/// </summary>
public record EmployeeInfo(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Position,
    string Status
); 