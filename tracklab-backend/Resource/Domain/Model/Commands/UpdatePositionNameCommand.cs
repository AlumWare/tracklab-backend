namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record UpdatePositionNameCommand(long PositionId, string NewName);