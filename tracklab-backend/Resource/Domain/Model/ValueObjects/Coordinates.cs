using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

[Owned]
public record Coordinates
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    public Coordinates() { } // Requerido por EF Core

    public Coordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public override string ToString() => $"({Latitude}, {Longitude})";
}