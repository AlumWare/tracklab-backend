namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyPositionNameException : Exception
{
    public NullOrEmptyPositionNameException() : base("Position name cannot be null or empty.") {}
    public NullOrEmptyPositionNameException(string message) : base(message) {}
    public NullOrEmptyPositionNameException(string message, Exception inner) : base(message, inner) {}
}