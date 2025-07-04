namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidWarehouseNameException : Exception
{
    public InvalidWarehouseNameException(string name) : base("Invalid warehouse name. Value: " + name) { }
}