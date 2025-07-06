namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidEmployeeStatusException : Exception
{
    public InvalidEmployeeStatusException() : base("Invalid employee status.") {}
    public InvalidEmployeeStatusException(string message) : base(message) {}
    public InvalidEmployeeStatusException(string message, Exception inner) : base(message, inner) {}
}