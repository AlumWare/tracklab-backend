namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal PriceAmount,
    string PriceCurrency
); 