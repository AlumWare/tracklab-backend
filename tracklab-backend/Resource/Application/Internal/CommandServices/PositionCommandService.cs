using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Domain.Repositories;
using Alumware.Tracklab.API.Resource.Domain.Services;
using TrackLab.Shared.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;

namespace Alumware.Tracklab.API.Resource.Application.Internal.CommandServices;

public class PositionCommandService : IPositionCommandService
{
    private readonly IPositionRepository _positionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public PositionCommandService(
        IPositionRepository positionRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _positionRepository = positionRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<Position> Handle(CreatePositionCommand command)
    {
        var position = new Position(command);
        
        // Establecer el tenant_id desde el contexto actual
        if (_tenantContext.HasTenant)
        {
            position.SetTenantId(_tenantContext.CurrentTenantId!.Value);
        }
        
        await _positionRepository.AddAsync(position);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
        return position;
    }

    public async Task Handle(UpdatePositionNameCommand command)
    {
        var position = await _positionRepository.FindByIdAsync(command.PositionId);
        if (position is null)
            throw new KeyNotFoundException($"Posición con ID {command.PositionId} no encontrada.");

        position.UpdateName(command);
        _positionRepository.Update(position);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }

    public async Task Handle(DeletePositionCommand command)
    {
        var position = await _positionRepository.FindByIdAsync(command.PositionId);
        if (position is null)
            throw new KeyNotFoundException($"Posición con ID {command.PositionId} no encontrada.");

        _positionRepository.Remove(position);
        await _unitOfWork.CompleteAsync(); // Persistir los cambios
    }
}