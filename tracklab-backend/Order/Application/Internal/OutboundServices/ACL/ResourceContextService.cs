using Alumware.Tracklab.API.Resource.Interfaces.ACL;

namespace Alumware.Tracklab.API.Order.Application.Internal.OutboundServices.ACL;

public class ResourceContextService : IResourceContextService
{
    private readonly IResourceContextFacade _resourceContextFacade;

    public ResourceContextService(IResourceContextFacade resourceContextFacade)
    {
        _resourceContextFacade = resourceContextFacade;
    }

    public async Task<bool> ValidateProductExistsAsync(long productId)
    {
        return await _resourceContextFacade.ValidateProductExistsAsync(productId);
    }

    public async Task<ProductInfo?> GetProductInfoAsync(long productId)
    {
        return await _resourceContextFacade.GetProductInfoAsync(productId);
    }

    public async Task<IEnumerable<ProductInfo>> GetProductsInfoAsync(IEnumerable<long> productIds)
    {
        return await _resourceContextFacade.GetProductsInfoAsync(productIds);
    }

    public async Task<bool> ValidateVehicleExistsAsync(long vehicleId)
    {
        return await _resourceContextFacade.ValidateVehicleExistsAsync(vehicleId);
    }

    public async Task<VehicleInfo?> GetVehicleInfoAsync(long vehicleId)
    {
        return await _resourceContextFacade.GetVehicleInfoAsync(vehicleId);
    }
} 