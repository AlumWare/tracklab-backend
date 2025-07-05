using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace TrackLab.Shared.Domain.ValueObjects;

[Owned]
public record Price
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;

    public Price() { } // Requerido por EF Core

    public Price(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));
        if (!Regex.IsMatch(currency, @"^[a-zA-Z]+$"))
            throw new ArgumentException("Currency must contain only letters", nameof(currency));
        Amount = amount;
        Currency = currency.ToUpper();
    }

    public override string ToString() => $"{Amount} {Currency}";

    public static implicit operator string(Price price) => price.ToString();
} 