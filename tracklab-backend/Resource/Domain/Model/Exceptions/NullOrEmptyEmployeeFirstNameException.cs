namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyEmployeeFirstNameException : Exception
{
    public NullOrEmptyEmployeeFirstNameException() : base("Employee first name cannot be null or empty.") {}
    public NullOrEmptyEmployeeFirstNameException(string message) : base(message) {}
    public NullOrEmptyEmployeeFirstNameException(string message, Exception inner) : base(message, inner) {}
}