namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidStatusValueException : Exception
{
    public InvalidStatusValueException(int statusValue) : base("Invalid status value. Value: " + statusValue) { }
}