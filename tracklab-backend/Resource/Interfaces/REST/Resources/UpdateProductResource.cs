namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record UpdateProductResource(
    string Name,
    string Description,
    decimal PriceAmount,
    string PriceCurrency,
    string Category,
    int Stock
); 