using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class AssignVehicleCommandFromResourceAssembler
{
    public static AssignVehicleCommand ToCommandFromResource(long orderId, AssignVehicleResource resource)
    {
        return new AssignVehicleCommand
        {
            OrderId = orderId,
            VehicleId = resource.VehicleId
        };
    }
} 