namespace Alumware.Tracklab.API.Tracking.Domain.Model.Commands;

/// <summary>
/// Command for completing a container when delivered at a CLIENT warehouse
/// </summary>
public record CompleteContainerCommand(
    long ContainerId,
    long DeliveryWarehouseId,
    DateTime DeliveryDate,
    string? DeliveryNotes = null
); 