namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidCoordinateLatitudeException : Exception
{
    public InvalidCoordinateLatitudeException(double latitude) : base("Invalid latitude coordinate. Value: " + latitude) { }
}