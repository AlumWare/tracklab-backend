namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidWarehouseTypeException : Exception
{
    public InvalidWarehouseTypeException(int type) : base("Invalid warehouse type. Value: " + type) { }
}