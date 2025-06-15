using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;
[Owned]
public record StreetAddress
{
    public string Value { get; init; }  

    protected StreetAddress() {}        // EF necesita constructor sin parámetros

    public StreetAddress(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;
}
