using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class SetRouteCommandFromResourceAssembler
{
    public static SetRouteCommand ToCommandFromResource(RouteResource resource)
    {
        return new SetRouteCommand(resource.OrderId, resource.VehicleId, resource.Warehouses);
    }
} 