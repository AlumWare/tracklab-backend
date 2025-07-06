namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidProductPriceAmountException : Exception
{
    public InvalidProductPriceAmountException() : base("Invalid product price amount.") {}
    public InvalidProductPriceAmountException(string message) : base(message) {}
    public InvalidProductPriceAmountException(string message, Exception inner) : base(message, inner) {}
}