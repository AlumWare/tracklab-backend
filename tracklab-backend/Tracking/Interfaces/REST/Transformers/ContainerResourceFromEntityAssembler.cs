using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;

public static class ContainerResourceFromEntityAssembler
{
    public static ContainerResource ToResourceFromEntity(Container container)
    {
        return new ContainerResource(
            container.ContainerId,
            container.OrderId.Value,
            container.WarehouseId.Value,
            container.ShipItems.Select(ToShipItemResourceFromEntity),
            container.TotalWeight
        );
    }

    public static IEnumerable<ContainerResource> ToResourceFromEntities(IEnumerable<Container> containers)
    {
        return containers.Select(ToResourceFromEntity);
    }

    private static ShipItemResource ToShipItemResourceFromEntity(Domain.Model.ValueObjects.ShipItem item)
    {
        return new ShipItemResource(
            item.ProductId,
            (int)item.Quantity,
            item.UnitWeight
        );
    }
} 