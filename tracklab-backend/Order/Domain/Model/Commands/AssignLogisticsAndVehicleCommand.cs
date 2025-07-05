namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;

public record AssignLogisticsAndVehicleCommand(
    long OrderId,
    long LogisticsId,
    long VehicleId
); 