using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class AssignLogisticsAndVehicleCommandFromResourceAssembler
{
    public static AssignLogisticsAndVehicleCommand ToCommandFromResource(long orderId, AssignLogisticsAndVehicleResource resource)
    {
        return new AssignLogisticsAndVehicleCommand(orderId, resource.LogisticsId, resource.VehicleId);
    }
} 