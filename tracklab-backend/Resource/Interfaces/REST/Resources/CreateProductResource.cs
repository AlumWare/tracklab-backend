namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record CreateProductResource(
    string Name,
    string Description,
    decimal PriceAmount,
    string PriceCurrency
); 