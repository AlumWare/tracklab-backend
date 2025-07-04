namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyStreetAddressException : Exception
{
    public NullOrEmptyStreetAddressException() : base("Address cannot be null or empty.") { }
}