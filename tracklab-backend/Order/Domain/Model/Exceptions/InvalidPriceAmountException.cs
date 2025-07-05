namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidPriceAmountException : Exception
{
    public InvalidPriceAmountException() : base("Invalid price amount for order.") {}
    public InvalidPriceAmountException(string message) : base(message) {}
    public InvalidPriceAmountException(string message, Exception inner) : base(message, inner) {}
}