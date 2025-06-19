using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

public class ContainerCommandService : IContainerCommandService
{
    private readonly IContainerRepository _containerRepository;

    public ContainerCommandService(IContainerRepository containerRepository)
    {
        _containerRepository = containerRepository;
    }

    public async Task<Container> CreateAsync(CreateContainerCommand command)
    {
        var container = new Container(command);
        await _containerRepository.AddAsync(container);
        return container;
    }

    public async Task<Container> UpdateCurrentNodeAsync(UpdateContainerNodeCommand command)
    {
        var container = await _containerRepository.GetByIdAsync(new GetContainerByIdQuery(command.ContainerId));
        if (container == null)
            throw new KeyNotFoundException($"Container {command.ContainerId} no encontrado.");
        container.UpdateCurrentNode(command);
        _containerRepository.Update(container);
        return container;
    }
} 