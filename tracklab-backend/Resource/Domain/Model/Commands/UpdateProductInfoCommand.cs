namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record UpdateProductInfoCommand(
    long ProductId,
    string Name,
    string Description,
    decimal PriceAmount,
    string PriceCurrency
); 