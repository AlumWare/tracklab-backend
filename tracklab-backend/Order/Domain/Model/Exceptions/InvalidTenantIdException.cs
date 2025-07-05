namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidTenantIdException : Exception
{
    public InvalidTenantIdException() : base("Invalid tenant ID for order.") {}
    public InvalidTenantIdException(string message) : base(message) {}
    public InvalidTenantIdException(string message, Exception inner) : base(message, inner) {}
}