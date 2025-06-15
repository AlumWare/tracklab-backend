using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IPositionQueryService
{
    Task<IEnumerable<Position>> ListAsync();
    Task<Position?> FindByIdAsync(long id);
}