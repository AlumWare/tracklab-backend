using Microsoft.AspNetCore.Http;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record AddVehicleImageCommand(
    long VehicleId,
    IFormFile ImageFile
); 