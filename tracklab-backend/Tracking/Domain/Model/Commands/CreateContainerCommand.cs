using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Commands;

public record CreateContainerCommand(
    long OrderId,
    long WarehouseId,
    decimal TotalWeight
); 