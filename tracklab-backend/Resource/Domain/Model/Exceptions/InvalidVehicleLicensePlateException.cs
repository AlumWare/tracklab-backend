namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidVehicleLicensePlateException : Exception
{
    public  InvalidVehicleLicensePlateException(string licensePlate) : base("Invalid vehicle license plate. Value: " + licensePlate) { }
}