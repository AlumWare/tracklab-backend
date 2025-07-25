﻿using Microsoft.AspNetCore.Http;

namespace Alumware.Tracklab.API.Resource.Interfaces.REST.Resources;

public record CreateVehicleResource(
    string LicensePlate,
    decimal LoadCapacity,
    int PaxCapacity,
    double Latitude,
    double Longitude,
    decimal Tonnage,
    IFormFile[]? Images = null
);