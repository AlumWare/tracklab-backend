namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehicleStatusException : Exception
{
    public InvalidVehicleStatusException() : base("Invalid vehicle status.") {}
    public InvalidVehicleStatusException(string message) : base(message) {}
    public InvalidVehicleStatusException(string message, Exception inner) : base(message, inner) {}
}