namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyEmployeeLastNameException : Exception
{
    public NullOrEmptyEmployeeLastNameException() : base("Employee last name cannot be null or empty.") {}
    public NullOrEmptyEmployeeLastNameException(string message) : base(message) {}
    public NullOrEmptyEmployeeLastNameException(string message, Exception inner) : base(message, inner) {}
}