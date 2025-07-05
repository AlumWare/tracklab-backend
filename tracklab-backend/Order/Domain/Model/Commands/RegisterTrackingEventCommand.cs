namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;

public class RegisterTrackingEventCommand
{
    public long OrderId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public long WarehouseId { get; set; }
    public DateTime EventTime { get; set; }
} 