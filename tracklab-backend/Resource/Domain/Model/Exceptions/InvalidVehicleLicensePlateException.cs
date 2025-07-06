namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehicleLicensePlateException : Exception
{
    public InvalidVehicleLicensePlateException() : base("Invalid vehicle license plate.") {}
    public InvalidVehicleLicensePlateException(string message) : base(message) {}
    public InvalidVehicleLicensePlateException(string message, Exception inner) : base(message, inner) {}
}