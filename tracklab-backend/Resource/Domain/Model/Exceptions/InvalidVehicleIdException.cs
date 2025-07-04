namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehicleIdException : Exception
{
    public InvalidVehicleIdException(long id) : base("Invalid vehicle id. Value: " + id) {}
}