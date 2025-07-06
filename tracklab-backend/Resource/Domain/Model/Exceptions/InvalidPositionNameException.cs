namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidPositionNameException : Exception
{
    public InvalidPositionNameException() : base("Invalid position name.") {}
    public InvalidPositionNameException(string message) : base(message) {}
    public InvalidPositionNameException(string message, Exception inner) : base(message, inner) {}
}