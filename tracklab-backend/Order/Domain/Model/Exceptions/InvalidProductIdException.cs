namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidProductIdException : Exception
{
    public InvalidProductIdException(long productId) : base("Invalid product id. ID: " + productId)
    {}
}