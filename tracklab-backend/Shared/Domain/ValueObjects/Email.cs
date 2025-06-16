using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace TrackLab.Shared.Domain.ValueObjects;

[Owned]
public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; init; }

    public Email() { } // requerido por EF

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be null or empty");
        
        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException("Invalid email format");

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
    public static explicit operator Email(string email) => new(email);
}
