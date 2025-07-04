using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;

namespace Alumware.Tracklab.API.Tracking.Domain.Services;

public interface IContainerQueryService
{
    Task<IEnumerable<Container>> Handle(GetAllContainersQuery query);
    Task<Container?> Handle(GetContainerByIdQuery query);
} 