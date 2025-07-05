namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidProductPriceCurrencyException : Exception
{
    public InvalidProductPriceCurrencyException() : base("Invalid product price currency.") {}
    public InvalidProductPriceCurrencyException(string message) : base(message) {}
    public InvalidProductPriceCurrencyException(string message, Exception inner) : base(message, inner) {}
}