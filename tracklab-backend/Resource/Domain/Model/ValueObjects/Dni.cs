using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

[Owned]
public record Dni
{
    public string Value { get; init; }

    public Dni() { } 

    public Dni(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("DNI cannot be empty");

        Value = value;
    }

    public override string ToString() => Value;
}