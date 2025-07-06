namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record VehicleImageResource(
    long Id,
    string ImageUrl,
    string PublicId,
    DateTime CreatedAt
); 