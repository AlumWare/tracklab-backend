namespace TrackLab.Resources.Domain.Model.ValueObjects;

/// <summary>
/// Coordinates value object for geographic location
/// </summary>
/// <param name="Latitude">Latitude in decimal degrees</param>
/// <param name="Longitude">Longitude in decimal degrees</param>
public readonly record struct Coordinates(double Latitude, double Longitude)
{
    public override string ToString() => $"{Latitude:F6}, {Longitude:F6}";
    
    public bool IsValid => Latitude >= -90 && Latitude <= 90 && Longitude >= -180 && Longitude <= 180;
} 