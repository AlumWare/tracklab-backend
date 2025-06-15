using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Domain.Model.Commands;

namespace Alumware.Tracklab.API.Resource.Domain.Services;

public interface IPositionCommandService
{
    Task<Position> Handle(CreatePositionCommand command);
    Task Handle(UpdatePositionNameCommand command);
    Task Handle(DeletePositionCommand command);
}