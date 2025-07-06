namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

public record ContainerResource(
    long ContainerId,
    long OrderId,
    long WarehouseId,
    IEnumerable<ShipItemResource> ShipItems,
    decimal TotalWeight
);

public record CreateContainerResource(
    long OrderId,
    long WarehouseId,
    decimal TotalWeight
);

public record ShipItemResource(
    long ProductId,
    int Quantity,
    decimal UnitWeight
); 