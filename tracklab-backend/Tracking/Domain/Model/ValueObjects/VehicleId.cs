using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Tracking.Domain.Model.ValueObjects;

[Owned]
public record VehicleId
{
    public long Value { get; private set; }

    public VehicleId() { }
    public VehicleId(long value)
    {
        if (value <= 0)
            throw new ArgumentException("El ID del vehículo debe ser mayor que 0", nameof(value));
        Value = value;
    }
    public override string ToString() => Value.ToString();
    public static implicit operator long(VehicleId id) => id.Value;
    public static explicit operator VehicleId(long value) => new(value);
} 