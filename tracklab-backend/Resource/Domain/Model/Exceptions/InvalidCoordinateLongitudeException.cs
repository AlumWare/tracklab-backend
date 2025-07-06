namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidCoordinateLongitudeException : Exception
{
    public InvalidCoordinateLongitudeException() : base("Invalid longitude coordinate.") {}
    public InvalidCoordinateLongitudeException(string message) : base(message) {}
    public InvalidCoordinateLongitudeException(string message, Exception inner) : base(message, inner) {}
}