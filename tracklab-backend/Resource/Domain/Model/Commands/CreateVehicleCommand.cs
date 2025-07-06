using Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Alumware.Tracklab.API.Resource.Domain.Model.Commands;

public record CreateVehicleCommand(
    string LicensePlate,
    decimal LoadCapacity,
    int PaxCapacity,
    Coordinates Location,
    decimal Tonnage,
    IFormFile[]? Images = null
);
