using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;

namespace Alumware.Tracklab.API.Resource.Application.Internal.QueryServices;

public class VehicleQueryService(IVehicleRepository vehicleRepository) : IVehicleQueryService
{
    public async Task<IEnumerable<Vehicle>> ListAsync()
    {
        return await vehicleRepository.ListAsync();
    }

    public async Task<Vehicle?> FindByIdAsync(long id)
    {
        return await vehicleRepository.FindByIdAsync(id);
    }
}