using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class UpdateProductInfoCommandFromResourceAssembler
{
    public static UpdateProductInfoCommand ToCommandFromResource(long productId, UpdateProductResource resource)
    {
        return new UpdateProductInfoCommand(
            productId,
            resource.Name,
            resource.Description,
            resource.PriceAmount,
            resource.PriceCurrency
        );
    }
} 