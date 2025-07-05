namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidWarehouseTypeException : Exception
{
    public InvalidWarehouseTypeException() : base("Invalid warehouse type.") {}
    public InvalidWarehouseTypeException(string message) : base(message) {}
    public InvalidWarehouseTypeException(string message, Exception inner) : base(message, inner) {}
}