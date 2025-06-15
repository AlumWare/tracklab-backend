using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class PositionCommandService : IPositionCommandService
{
    private readonly IPositionRepository _positionRepository;

    public PositionCommandService(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public async Task<Position> Handle(CreatePositionCommand command)
    {
        var position = new Position(command);
        await _positionRepository.AddAsync(position);
        return position;
    }

    public async Task Handle(UpdatePositionNameCommand command)
    {
        var position = await _positionRepository.FindByIdAsync(command.PositionId);
        if (position is null)
            throw new KeyNotFoundException($"Posición con ID {command.PositionId} no encontrada.");

        position.UpdateName(command);
        _positionRepository.Update(position);
    }

    public async Task Handle(DeletePositionCommand command)
    {
        var position = await _positionRepository.FindByIdAsync(command.PositionId);
        if (position is null)
            throw new KeyNotFoundException($"Posición con ID {command.PositionId} no encontrada.");

        _positionRepository.Remove(position);
    }
}