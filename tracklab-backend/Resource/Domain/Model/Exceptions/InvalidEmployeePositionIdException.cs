namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidEmployeePositionIdException : Exception
{
    public InvalidEmployeePositionIdException() : base("Invalid employee position ID.") {}
    public InvalidEmployeePositionIdException(string message) : base(message) {}
    public InvalidEmployeePositionIdException(string message, Exception inner) : base(message, inner) {}
}