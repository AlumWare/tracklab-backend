namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyWarehouseNameException : Exception
{
    public  NullOrEmptyWarehouseNameException() : base("Warehouse name cannot be null or empty.") { }
}