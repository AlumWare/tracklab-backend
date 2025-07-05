namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidPositionIdException : Exception
{
    public InvalidPositionIdException() : base("Invalid position ID.") {}
    public InvalidPositionIdException(string message) : base(message) {}
    public InvalidPositionIdException(string message, Exception inner) : base(message, inner) {}
}