using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace TrackLab.Shared.Domain.ValueObjects;

[Owned]
public record PhoneNumber
{
    private static readonly Regex PhoneRegex = new(
        @"^\+?[1-9]\d{1,14}$", // Formato internacional básico
        RegexOptions.Compiled);

    public string Value { get; init; }

    public PhoneNumber() { } // requerido por EF

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number cannot be null or empty");

        // Limpiar el número (remover espacios, guiones, paréntesis)
        var cleanedNumber = Regex.Replace(value, @"[\s\-\(\)]", "");
        
        if (!PhoneRegex.IsMatch(cleanedNumber))
            throw new ArgumentException("Invalid phone number format");

        Value = cleanedNumber;
    }

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phone) => phone.Value;
    public static explicit operator PhoneNumber(string phone) => new(phone);
}
