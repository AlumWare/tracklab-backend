namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyShippingAddressException : Exception
{
    public NullOrEmptyShippingAddressException() : base("Shipping address cannot be null or empty.") {}
    public NullOrEmptyShippingAddressException(string message) : base(message) {}
    public NullOrEmptyShippingAddressException(string message, Exception inner) : base(message, inner) {}
}