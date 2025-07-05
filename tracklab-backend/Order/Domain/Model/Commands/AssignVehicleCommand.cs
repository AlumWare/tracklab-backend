namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;

public class AssignVehicleCommand
{
    public long OrderId { get; set; }
    public long VehicleId { get; set; }
} 