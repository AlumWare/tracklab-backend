namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehicleLoadCapacityException : Exception
{
    public InvalidVehicleLoadCapacityException() : base("Invalid vehicle load capacity.") {}
    public InvalidVehicleLoadCapacityException(string message) : base(message) {}
    public InvalidVehicleLoadCapacityException(string message, Exception inner) : base(message, inner) {}
}