namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record ProductResource(
    long Id,
    string Name,
    string Description,
    decimal PriceAmount,
    string PriceCurrency
); 