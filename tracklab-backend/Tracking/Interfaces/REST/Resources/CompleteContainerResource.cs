namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

/// <summary>
/// Resource for completing a container when delivered at a CLIENT warehouse
/// </summary>
public record CompleteContainerResource(
    long DeliveryWarehouseId,
    DateTime? DeliveryDate = null,
    string? DeliveryNotes = null
); 