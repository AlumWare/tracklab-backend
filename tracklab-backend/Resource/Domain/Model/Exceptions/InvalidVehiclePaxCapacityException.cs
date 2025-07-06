namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehiclePaxCapacityException : Exception
{
    public InvalidVehiclePaxCapacityException() : base("Invalid vehicle passenger capacity.") {}
    public InvalidVehiclePaxCapacityException(string message) : base(message) {}
    public InvalidVehiclePaxCapacityException(string message, Exception inner) : base(message, inner) {}
}