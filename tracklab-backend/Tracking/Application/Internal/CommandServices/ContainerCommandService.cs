using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.Queries;
using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Domain.Repositories;
using Alumware.Tracklab.API.Tracking.Domain.Services;
using TrackLab.Shared.Domain.Repositories;

namespace Alumware.Tracklab.API.Tracking.Application.Internal.CommandServices;

public class ContainerCommandService : IContainerCommandService
{
    private readonly IContainerRepository _containerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ContainerCommandService(
        IContainerRepository containerRepository,
        IUnitOfWork unitOfWork)
    {
        _containerRepository = containerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Container> CreateAsync(CreateContainerCommand command)
    {
        var container = new Container(command);
        await _containerRepository.AddAsync(container);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return container;
    }

    public async Task<Container> UpdateCurrentNodeAsync(UpdateContainerNodeCommand command)
    {
        var container = await _containerRepository.GetByIdAsync(new GetContainerByIdQuery(command.ContainerId));
        if (container == null)
            throw new KeyNotFoundException($"Container {command.ContainerId} no encontrado.");
        container.UpdateCurrentNode(command);
        _containerRepository.Update(container);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return container;
    }
} 