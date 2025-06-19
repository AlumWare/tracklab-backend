namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

public record ContainerResource(
    long ContainerId,
    long OrderId,
    long WarehouseId,
    IEnumerable<ShipItemResource> ShipItems
);

public record ShipItemResource(
    long ProductId,
    long Quantity
); 