using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Interfaces.ACL;

namespace Alumware.Tracklab.API.Resource.Application.ACL;

public class ResourceContextFacade : IResourceContextFacade
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPositionRepository _positionRepository;

    public ResourceContextFacade(
        IProductRepository productRepository,
        IWarehouseRepository warehouseRepository,
        IVehicleRepository vehicleRepository,
        IEmployeeRepository employeeRepository,
        IPositionRepository positionRepository)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
        _vehicleRepository = vehicleRepository;
        _employeeRepository = employeeRepository;
        _positionRepository = positionRepository;
    }

    public async Task<bool> ValidateProductExistsAsync(long productId)
    {
        var product = await _productRepository.FindByIdAsync(productId);
        return product != null && product.Stock > 0;
    }

    public async Task<ProductInfo?> GetProductInfoAsync(long productId)
    {
        var product = await _productRepository.FindByIdAsync(productId);
        if (product == null)
            return null;

        return new ProductInfo(
            product.Id,
            product.Name,
            product.Description,
            product.Price.Amount,
            product.Price.Currency,
            product.Category,
            product.Stock
        );
    }

    public async Task<IEnumerable<ProductInfo>> GetProductsInfoAsync(IEnumerable<long> productIds)
    {
        var products = new List<ProductInfo>();
        
        foreach (var productId in productIds)
        {
            var productInfo = await GetProductInfoAsync(productId);
            if (productInfo != null)
            {
                products.Add(productInfo);
            }
        }
        
        return products;
    }

    public async Task<bool> ValidateWarehouseExistsAsync(long warehouseId)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(warehouseId);
        return warehouse != null;
    }

    public async Task<WarehouseInfo?> GetWarehouseInfoAsync(long warehouseId)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(warehouseId);
        if (warehouse == null)
            return null;

        return new WarehouseInfo(
            warehouse.Id,
            warehouse.Name,
            warehouse.Address.Value,
            warehouse.Type.ToString(),
            warehouse.Coordinates.Latitude,
            warehouse.Coordinates.Longitude
        );
    }

    public async Task<Alumware.Tracklab.API.Resource.Domain.Model.Aggregates.Warehouse?> GetWarehouseAsync(long warehouseId)
    {
        return await _warehouseRepository.FindByIdAsync(warehouseId);
    }

    public async Task<bool> ValidateVehicleExistsAsync(long vehicleId)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(vehicleId);
        return vehicle != null && vehicle.Status == Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects.EVehicleStatus.Available;
    }

    public async Task<VehicleInfo?> GetVehicleInfoAsync(long vehicleId)
    {
        var vehicle = await _vehicleRepository.FindByIdAsync(vehicleId);
        if (vehicle == null)
            return null;

        return new VehicleInfo(
            vehicle.Id,
            vehicle.LicensePlate,
            vehicle.LoadCapacity,
            vehicle.PaxCapacity,
            vehicle.Status.ToString(),
            vehicle.Tonnage,
            vehicle.Location.Latitude,
            vehicle.Location.Longitude
        );
    }

    public async Task<bool> ValidateEmployeeExistsAsync(long employeeId)
    {
        var employee = await _employeeRepository.FindByIdAsync(employeeId);
        return employee != null && employee.IsAvailable();
    }

    public async Task<EmployeeInfo?> GetEmployeeInfoAsync(long employeeId)
    {
        var employee = await _employeeRepository.FindByIdAsync(employeeId);
        if (employee == null)
            return null;

        return new EmployeeInfo(
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email.Value,
            employee.PhoneNumber,
            employee.Position?.Name ?? "Unknown",
            employee.Status.ToString()
        );
    }

    public async Task<bool> ValidatePositionExistsAsync(long positionId)
    {
        var position = await _positionRepository.FindByIdAsync(positionId);
        return position != null;
    }
} 