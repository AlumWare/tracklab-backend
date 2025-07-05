namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidProductIdException : Exception
{
    public InvalidProductIdException() : base("Invalid product ID for order.") {}
    public InvalidProductIdException(string message) : base(message) {}
    public InvalidProductIdException(string message, Exception inner) : base(message, inner) {}
}