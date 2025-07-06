using Alumware.Tracklab.API.Resource.Domain.Model.Aggregates;
using Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Transformers;

public static class ProductResourceFromEntityAssembler
{
    public static ProductResource ToResourceFromEntity(Product product)
    {
        return new ProductResource(
            product.Id,
            product.Name,
            product.Description,
            product.Price.Amount,
            product.Price.Currency,
            product.Category,
            product.Stock
        );
    }

    public static IEnumerable<ProductResource> ToResourceFromEntities(IEnumerable<Product> products)
    {
        return products.Select(ToResourceFromEntity);
    }
} 