namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class NullOrderItemQuantityException : Exception
{
    public NullOrderItemQuantityException() : base("Order item quantity cannot be null or zero.") {}
    public NullOrderItemQuantityException(string message) : base(message) {}
    public NullOrderItemQuantityException(string message, Exception inner) : base(message, inner) {}
}