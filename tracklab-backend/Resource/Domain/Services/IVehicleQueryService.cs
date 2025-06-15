using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IVehicleQueryService
{
    Task<IEnumerable<Vehicle>> ListAsync();
    Task<Vehicle?> FindByIdAsync(long id);
}