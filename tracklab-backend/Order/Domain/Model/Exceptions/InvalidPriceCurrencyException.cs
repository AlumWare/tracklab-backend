namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidPriceCurrencyException : Exception
{
    public InvalidPriceCurrencyException() : base("Invalid price currency for order.") {}
    public InvalidPriceCurrencyException(string message) : base(message) {}
    public InvalidPriceCurrencyException(string message, Exception inner) : base(message, inner) {}
}