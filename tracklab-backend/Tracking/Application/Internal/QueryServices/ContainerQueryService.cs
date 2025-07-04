using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.QueryServices;

public class ContainerQueryService : IContainerQueryService
{
    private readonly IContainerRepository _containerRepository;

    public ContainerQueryService(IContainerRepository containerRepository)
    {
        _containerRepository = containerRepository;
    }

    public async Task<IEnumerable<Container>> Handle(GetAllContainersQuery query)
    {
        return await _containerRepository.GetAllAsync(query);
    }

    public async Task<Container?> Handle(GetContainerByIdQuery query)
    {
        return await _containerRepository.GetByIdAsync(query);
    }
} 