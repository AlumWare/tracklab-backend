using Microsoft.EntityFrameworkCore;

namespace Alumware.Tracklab.API.Resource.Domain.Model.ValueObjects;

[Owned]
public record Email
{
    public string Value { get; private set; } = null!;

    public Email() { } 

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El email no puede estar vacío", nameof(value));
        
        if (!IsValidEmail(value))
            throw new ArgumentException("El formato del email no es válido", nameof(value));
        
        Value = value;
    }

    private static bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    public override string ToString() => Value;
}