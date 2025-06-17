using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

[Owned]
public record Dni
{
    public string Value { get; private set; } = null!;

    public Dni() { } 

    public Dni(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El DNI no puede estar vacío", nameof(value));
        
        Value = value;
    }

    public override string ToString() => Value;
}