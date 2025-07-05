namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidWarehouseNameException : Exception
{
    public InvalidWarehouseNameException() : base("Invalid warehouse name.") {}
    public InvalidWarehouseNameException(string message) : base(message) {}
    public InvalidWarehouseNameException(string message, Exception inner) : base(message, inner) {}
}