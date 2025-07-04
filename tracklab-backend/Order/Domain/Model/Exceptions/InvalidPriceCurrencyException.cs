namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidPriceCurrencyException : Exception
{
    public InvalidPriceCurrencyException(string priceCurrency) : base("Invalid price currency. Value: " + priceCurrency) { }
}