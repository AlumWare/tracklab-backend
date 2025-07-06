namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

public record LogisticsCompanyResource(
    long Id,
    string LegalName,
    string? CommercialName,
    string? Address,
    string? City,
    string? Country,
    string? Email,
    string? Phone,
    string? Website
); 