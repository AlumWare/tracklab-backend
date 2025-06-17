using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace TrackLab.Shared.Domain.ValueObjects;

[Owned]
public record PhoneNumber
{
    private static readonly Regex PhoneRegex = new(
        @"^\+?[1-9]\d{1,14}$", // Formato internacional básico
        RegexOptions.Compiled);

    public string Value { get; private set; } = null!;

    public PhoneNumber() { } // requerido por EF

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El número de teléfono no puede estar vacío", nameof(value));
        
        if (!PhoneRegex.IsMatch(value))
            throw new ArgumentException("El formato del número de teléfono no es válido", nameof(value));
        
        Value = value;
    }

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phone) => phone.Value;
    public static explicit operator PhoneNumber(string phone) => new(phone);
}
