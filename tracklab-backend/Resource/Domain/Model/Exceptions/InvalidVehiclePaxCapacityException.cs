namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehiclePaxCapacityException : Exception
{
    public InvalidVehiclePaxCapacityException(int paxCapacity) : base("Invalid vehicle pax capacity. Value: " + paxCapacity) { }
}