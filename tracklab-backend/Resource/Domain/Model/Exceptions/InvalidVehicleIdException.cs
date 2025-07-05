namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehicleIdException : Exception
{
    public InvalidVehicleIdException() : base("Invalid vehicle ID.") {}
    public InvalidVehicleIdException(string message) : base(message) {}
    public InvalidVehicleIdException(string message, Exception inner) : base(message, inner) {}
}