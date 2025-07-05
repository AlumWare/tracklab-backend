namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidCoordinateLatitudeException : Exception
{
    public InvalidCoordinateLatitudeException() : base("Invalid latitude coordinate.") {}
    public InvalidCoordinateLatitudeException(string message) : base(message) {}
    public InvalidCoordinateLatitudeException(string message, Exception inner) : base(message, inner) {}
}