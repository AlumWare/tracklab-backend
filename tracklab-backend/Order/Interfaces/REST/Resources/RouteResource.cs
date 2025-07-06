namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

public record RouteResource(
    long OrderId,
    long VehicleId,
    List<long> Warehouses // IDs de almacenes en orden
); 