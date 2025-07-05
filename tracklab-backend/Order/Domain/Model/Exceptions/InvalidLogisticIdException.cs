namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidLogisticIdException : Exception
{
    public InvalidLogisticIdException() : base("Invalid logistic ID for order.") {}
    public InvalidLogisticIdException(string message) : base(message) {}
    public InvalidLogisticIdException(string message, Exception inner) : base(message, inner) {}
}