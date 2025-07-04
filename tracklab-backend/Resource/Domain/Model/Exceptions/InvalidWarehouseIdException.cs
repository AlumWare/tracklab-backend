namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidWarehouseIdException : Exception
{
    public InvalidWarehouseIdException(int id) : base("Invalid warehouse id. Value: " + id) { }
}