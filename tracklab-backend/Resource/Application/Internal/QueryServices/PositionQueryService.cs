using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;

namespace Alumware.Tracklab.API.Resource.Application.Internal.QueryServices;

public class PositionQueryService : IPositionQueryService
{
    private readonly IPositionRepository _positionRepository;

    public PositionQueryService(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public async Task<IEnumerable<Position>> ListAsync()
    {
        return await _positionRepository.ListAsync();
    }

    public async Task<Position?> FindByIdAsync(long id)
    {
        return await _positionRepository.FindByIdAsync(id);
    }
}
