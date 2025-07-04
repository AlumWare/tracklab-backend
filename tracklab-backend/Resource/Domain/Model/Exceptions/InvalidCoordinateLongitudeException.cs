namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidCoordinateLongitudeException : Exception
{
    public InvalidCoordinateLongitudeException(double longitude) : base("Invalid longitude coordinate. Value: " + longitude) { }
}