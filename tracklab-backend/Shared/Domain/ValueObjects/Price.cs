using Microsoft.EntityFrameworkCore;

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
            throw new ArgumentException("El monto no puede ser negativo", nameof(amount));
        
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("La moneda no puede estar vacía", nameof(currency));
        
        Amount = amount;
        Currency = currency.ToUpper();
    }

    public override string ToString() => $"{Amount} {Currency}";

    public static implicit operator string(Price price) => price.ToString();
} 