using Alumware.Tracklab.API.Resource.Interfaces.ACL;
using Microsoft.Extensions.Logging;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.OutboundServices.ACL;

public class ResourceContextService : IResourceContextService
{
    private readonly IResourceContextFacade _resourceContextFacade;
    private readonly ILogger<ResourceContextService> _logger;

    public ResourceContextService(IResourceContextFacade resourceContextFacade, ILogger<ResourceContextService> logger)
    {
        _resourceContextFacade = resourceContextFacade;
        _logger = logger;
    }

    public async Task<bool> ValidateVehicleExistsAsync(long vehicleId)
    {
        return await _resourceContextFacade.ValidateVehicleExistsAsync(vehicleId);
    }

    public async Task<bool> ValidateWarehouseExistsAsync(long warehouseId)
    {
        return await _resourceContextFacade.ValidateWarehouseExistsAsync(warehouseId);
    }

    public async Task<bool> ValidateEmployeeExistsAsync(long employeeId)
    {
        return await _resourceContextFacade.ValidateEmployeeExistsAsync(employeeId);
    }

    public async Task<Alumware.Tracklab.API.Resource.Interfaces.ACL.WarehouseInfo?> GetWarehouseInfoAsync(long warehouseId)
    {
        return await _resourceContextFacade.GetWarehouseInfoAsync(warehouseId);
    }

    public async Task<Alumware.Tracklab.API.Resource.Interfaces.ACL.VehicleInfo?> GetVehicleInfoAsync(long vehicleId)
    {
        return await _resourceContextFacade.GetVehicleInfoAsync(vehicleId);
    }

    public async Task<Alumware.Tracklab.API.Resource.Interfaces.ACL.EmployeeInfo?> GetEmployeeInfoAsync(long employeeId)
    {
        return await _resourceContextFacade.GetEmployeeInfoAsync(employeeId);
    }

    /// <summary>
    /// Gets warehouse information including type
    /// </summary>
    public async Task<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Warehouse?> GetWarehouseAsync(long warehouseId)
    {
        try
        {
            return await _resourceContextFacade.GetWarehouseAsync(warehouseId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting warehouse {WarehouseId}", warehouseId);
            return null;
        }
    }
} 