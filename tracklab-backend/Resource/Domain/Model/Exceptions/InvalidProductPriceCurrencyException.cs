namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidProductPriceCurrencyException : Exception
{
    public InvalidProductPriceCurrencyException(string priceCurrency) : base("Invalid product price currency. Value: " + priceCurrency) { }
}