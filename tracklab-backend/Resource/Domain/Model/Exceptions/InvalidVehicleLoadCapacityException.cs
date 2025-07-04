namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehicleLoadCapacityException : Exception
{
    public InvalidVehicleLoadCapacityException(decimal loadCapacity) : base("Invalid vehicle load capacity. Value: " + loadCapacity) {}
}