using Alumware.Tracklab.API.Tracking.Domain.Model.Commands;
using Alumware.Tracklab.API.Tracking.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Tracking.Interfaces.REST.Transformers;

/// <summary>
/// Assembler to transform CreateRouteResource to CreateRouteCommand
/// </summary>
public static class CreateRouteCommandFromResourceAssembler
{
    /// <summary>
    /// Transforms a CreateRouteResource to CreateRouteCommand
    /// </summary>
    /// <param name="resource">The CreateRouteResource to transform</param>
    /// <returns>CreateRouteCommand with route planning information</returns>
    public static CreateRouteCommand ToCommandFromResource(CreateRouteResource resource)
    {
        return new CreateRouteCommand(
            resource.VehicleId,
            resource.RouteName,
            resource.WarehouseIds,
            resource.OrderIds,
            resource.PlannedStartDate,
            resource.Description
        );
    }
} 