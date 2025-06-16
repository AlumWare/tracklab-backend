using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;
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

    public async Task<IEnumerable<Position>> Handle(GetAllPositionsQuery query)
    {
        var positions = await _positionRepository.ListAsync();
        
        // Nota: Los filtros Department e IsActive no existen en el modelo actual
        // Solo aplicamos paginación por ahora
        
        // Aplicar paginación si está especificada
        if (query.PageSize.HasValue && query.PageNumber.HasValue)
        {
            positions = positions
                .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                .Take(query.PageSize.Value);
        }
        
        return positions;
    }

    public async Task<Position?> Handle(GetPositionByIdQuery query)
    {
        return await _positionRepository.FindByIdAsync(query.Id);
    }
}
