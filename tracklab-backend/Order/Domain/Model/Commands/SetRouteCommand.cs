namespace Alumware.Tracklab.API.Order.Domain.Model.Commands;
using System.Collections.Generic;

public record SetRouteCommand(
    long OrderId,
    long VehicleId,
    List<long> Warehouses
); 