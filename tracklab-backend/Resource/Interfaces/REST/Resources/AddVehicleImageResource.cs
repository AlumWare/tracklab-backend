using Microsoft.AspNetCore.Http;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record AddVehicleImageResource(
    IFormFile ImageFile
); 