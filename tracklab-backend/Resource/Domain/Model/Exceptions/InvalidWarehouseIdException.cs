namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidWarehouseIdException : Exception
{
    public InvalidWarehouseIdException() : base("Invalid warehouse ID.") {}
    public InvalidWarehouseIdException(string message) : base(message) {}
    public InvalidWarehouseIdException(string message, Exception inner) : base(message, inner) {}
}