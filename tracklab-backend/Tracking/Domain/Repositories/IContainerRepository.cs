using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using TrackLab.Shared.Domain.Repositories;

namespace Alumware.Tracklab.API.Tracking.Domain.Repositories;

public interface IContainerRepository : IBaseRepository<Container>
{
    Task<IEnumerable<Container>> GetAllAsync(GetAllContainersQuery query);
    Task<Container?> GetByIdAsync(GetContainerByIdQuery query);
} 