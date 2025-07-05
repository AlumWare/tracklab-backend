namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyStreetAddressException : Exception
{
    public NullOrEmptyStreetAddressException() : base("Street address cannot be null or empty.") {}
    public NullOrEmptyStreetAddressException(string message) : base(message) {}
    public NullOrEmptyStreetAddressException(string message, Exception inner) : base(message, inner) {}
}