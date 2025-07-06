using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;

public static class CreateContainerCommandFromResourceAssembler
{
    public static CreateContainerCommand ToCommandFromResource(CreateContainerResource resource)
    {
        return new CreateContainerCommand(
            resource.OrderId,
            resource.WarehouseId,
            resource.ShipItems.Select(ToShipItemCommandFromResource),
            resource.TotalWeight
        );
    }

    private static CreateShipItemCommand ToShipItemCommandFromResource(CreateShipItemResource resource)
    {
        return new CreateShipItemCommand(
            resource.ProductId,
            resource.Quantity,
            resource.UnitWeight
        );
    }
} 