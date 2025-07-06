namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public record GetAvailableProductsQuery(
    int? PageSize = null,
    int? PageNumber = null,
    string? Name = null,
    string? Category = null
); 