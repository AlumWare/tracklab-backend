namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyShippingAddressException : Exception
{
    public NullOrEmptyShippingAddressException() : base("Shipping address can't be empty.")
    {
    }
}