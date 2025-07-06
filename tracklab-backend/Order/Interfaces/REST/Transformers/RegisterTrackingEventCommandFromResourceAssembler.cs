using Alumware.Tracklab.API.Order.Domain.Model.Commands;
using Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Transformers;

public static class RegisterTrackingEventCommandFromResourceAssembler
{
    public static RegisterTrackingEventCommand ToCommand(TrackingEventResource resource)
    {
        return new RegisterTrackingEventCommand
        {
            OrderId = resource.OrderId,
            EventType = resource.EventType,
            WarehouseId = resource.WarehouseId,
            EventTime = resource.EventTime
        };
    }
} 