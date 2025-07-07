using Alumware.Tracklab.API.Tracking.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;

/// <summary>
/// Assembler to transform Container entities to ContainerResource DTOs
/// </summary>
public static class ContainerResourceFromEntityAssembler
{
    /// <summary>
    /// Transforms a Container entity to ContainerResource
    /// </summary>
    public static ContainerResource ToResourceFromEntity(Container container)
    {
        return new ContainerResource(
            container.ContainerId,
            container.OrderId.Value,
            container.WarehouseId.Value,
            container.ShipItems.Select(ToShipItemResourceFromEntity),
            container.TotalWeight,
            container.QrCode != null ? ToQrCodeResourceFromEntity(container.QrCode) : null,
            container.IsCompleted,
            container.CompletedAt,
            container.CompletionNotes
        );
    }

    /// <summary>
    /// Transforms multiple Container entities to ContainerResource collection
    /// </summary>
    public static IEnumerable<ContainerResource> ToResourceFromEntities(IEnumerable<Container> containers)
    {
        return containers.Select(ToResourceFromEntity);
    }

    /// <summary>
    /// Transforms ShipItem value object to ShipItemResource
    /// </summary>
    private static ShipItemResource ToShipItemResourceFromEntity(Domain.Model.ValueObjects.ShipItem item)
    {
        return new ShipItemResource(
            item.ProductId,
            (int)item.Quantity,
            item.UnitWeight
        );
    }

    /// <summary>
    /// Transforms QrCode value object to QrCodeResource
    /// </summary>
    private static QrCodeResource ToQrCodeResourceFromEntity(Domain.Model.ValueObjects.QrCode qrCode)
    {
        return new QrCodeResource(
            qrCode.Url,
            qrCode.GeneratedAt
        );
    }
} 