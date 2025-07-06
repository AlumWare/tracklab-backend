namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

/// <summary>
/// Resource representing a container with delivery tracking information
/// </summary>
public record ContainerResource(
    long ContainerId,
    long OrderId,
    long WarehouseId,
    IEnumerable<ShipItemResource> ShipItems,
    decimal TotalWeight,
    QrCodeResource? QrCode,
    bool IsCompleted,
    DateTime? CompletedAt,
    string? CompletionNotes
);

/// <summary>
/// Resource for creating a new container
/// </summary>
public record CreateContainerResource(
    long OrderId,
    long WarehouseId,
    IEnumerable<CreateShipItemResource> ShipItems,
    decimal TotalWeight = 0
);

/// <summary>
/// Resource representing a ship item in a container
/// </summary>
public record ShipItemResource(
    long ProductId,
    int Quantity,
    decimal UnitWeight
);

/// <summary>
/// Resource for creating a ship item
/// </summary>
public record CreateShipItemResource(
    long ProductId,
    int Quantity,
    decimal UnitWeight = 1.0m
);

/// <summary>
/// Resource representing QR code information
/// </summary>
public record QrCodeResource(
    string Url,
    DateTime GeneratedAt
); 