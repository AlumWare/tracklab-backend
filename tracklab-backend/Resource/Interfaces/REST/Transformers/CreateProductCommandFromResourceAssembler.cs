using Alumware.Tracklab.API.Resource.Domain.Model.Commands;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class CreateProductCommandFromResourceAssembler
{
    public static CreateProductCommand ToCommandFromResource(CreateProductResource resource)
    {
        return new CreateProductCommand(
            resource.Name,
            resource.Description,
            resource.PriceAmount,
            resource.PriceCurrency
        );
    }
} 