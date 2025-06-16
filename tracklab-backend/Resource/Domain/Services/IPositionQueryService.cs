using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Queries;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IPositionQueryService
{
    Task<IEnumerable<Position>> Handle(GetAllPositionsQuery query);
    Task<Position?> Handle(GetPositionByIdQuery query);
}