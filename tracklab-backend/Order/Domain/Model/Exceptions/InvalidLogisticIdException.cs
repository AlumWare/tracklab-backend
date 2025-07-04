namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidLogisticIdException : Exception
{
    public InvalidLogisticIdException(long logisticId) : base("Invalid logistic ID. ID: " + logisticId) {}
}