namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehicleStatusException : Exception
{
    public InvalidVehicleStatusException(int status) : base("Invalid vehicle status. Value: " + status) { }
}