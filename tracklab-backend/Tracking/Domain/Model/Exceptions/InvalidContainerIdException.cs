namespace Alumware.Tracklab.API.Tracking.Domain.Model.Exceptions;

[Serializable]
public class InvalidContainerIdException : Exception
{
    public InvalidContainerIdException() : base("Invalid container ID.") {}
    public InvalidContainerIdException(string message) : base(message) {}
    public InvalidContainerIdException(string message, Exception inner) : base(message, inner) {}
}