namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class NullOrderItemQuantityException : Exception
{
    public NullOrderItemQuantityException() : base("Item quantity can't be zero or null.")
    {}
}