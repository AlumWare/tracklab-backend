namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

public record OrderTrackingInfoResource(
    long OrderId,
    int TotalContainers,
    int DeliveredContainers,
    int InTransitContainers,
    int PendingContainers,
    string OverallStatus,
    DateTime? LastUpdated
); 