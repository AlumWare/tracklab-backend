namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyVehicleLicensePlateException : Exception
{
    public NullOrEmptyVehicleLicensePlateException() : base("Vehicle license plate cannot be null or empty.") { }
}