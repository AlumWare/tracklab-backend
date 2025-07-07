using Alumware.Tracklab.API.Resource.Interfaces.ACL;

namespace Alumware.Tracklab.API.Order.Application.Internal.OutboundServices.ACL;

public interface IResourceContextService
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
    /// Validates if a vehicle exists and is available
    /// </summary>
    Task<bool> ValidateVehicleExistsAsync(long vehicleId);
    
    /// <summary>
    /// Gets basic vehicle information
    /// </summary>
    Task<VehicleInfo?> GetVehicleInfoAsync(long vehicleId);
} 