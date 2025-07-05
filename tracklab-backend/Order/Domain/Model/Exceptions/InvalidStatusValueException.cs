namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidStatusValueException : Exception
{
    public InvalidStatusValueException() : base("Invalid status value for order.") {}
    public InvalidStatusValueException(string message) : base(message) {}
    public InvalidStatusValueException(string message, Exception inner) : base(message, inner) {}
}