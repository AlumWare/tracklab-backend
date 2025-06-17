using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

[Owned]
public record StreetAddress
{
    public string Value { get; private set; } = null!;

    public StreetAddress() {}        // EF necesita constructor sin parámetros

    public StreetAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("La dirección no puede estar vacía", nameof(value));
        
        Value = value;
    }

    public override string ToString() => Value;
}
