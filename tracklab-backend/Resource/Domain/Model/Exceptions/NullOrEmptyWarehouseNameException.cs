namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyWarehouseNameException : Exception
{
    public NullOrEmptyWarehouseNameException() : base("Warehouse name cannot be null or empty.") {}
    public NullOrEmptyWarehouseNameException(string message) : base(message) {}
    public NullOrEmptyWarehouseNameException(string message, Exception inner) : base(message, inner) {}
}