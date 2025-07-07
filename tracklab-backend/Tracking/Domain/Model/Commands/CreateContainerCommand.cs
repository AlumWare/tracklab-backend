using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.Commands;

public record CreateContainerCommand(
    long OrderId,
    long WarehouseId,
    IEnumerable<CreateShipItemCommand> ShipItems,
    decimal TotalWeight
);

public record CreateShipItemCommand(
    long ProductId,
    int Quantity,
    decimal UnitWeight
); 