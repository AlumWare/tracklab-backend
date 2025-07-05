namespace Alumware.Tracklab.API.Order.Domain.Model.Aggregates;

public class TrackingEvent
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public long WarehouseId { get; set; }
    public DateTime EventTime { get; set; }
    public int Sequence { get; set; } // Para validar secuencia de eventos
} 