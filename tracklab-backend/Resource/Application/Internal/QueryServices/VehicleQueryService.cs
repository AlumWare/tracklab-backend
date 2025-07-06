using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;

namespace Alumware.Tracklab.API.Resource.Application.Internal.QueryServices;

public class VehicleQueryService : IVehicleQueryService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleQueryService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesQuery query)
    {
        var vehicles = await _vehicleRepository.ListAsync();
        
        // Aplicar filtros si están especificados
        if (!string.IsNullOrEmpty(query.Status))
        {
            if (Enum.TryParse<EVehicleStatus>(query.Status, out var status))
            {
                vehicles = vehicles.Where(v => v.Status == status);
            }
        }
        
        if (!string.IsNullOrEmpty(query.LicensePlate))
        {
            vehicles = vehicles.Where(v => v.LicensePlate.Contains(query.LicensePlate, StringComparison.OrdinalIgnoreCase));
        }
        
        
        // Aplicar paginación si está especificada
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
        {
            vehicles = vehicles
                .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                .Take(query.PageSize.Value);
        }
        
        return vehicles;
    }

    public async Task<Vehicle?> Handle(GetVehicleByIdQuery query)
    {
        return await _vehicleRepository.FindByIdAsync(query.Id);
    }
}