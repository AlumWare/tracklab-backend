namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyEmployeeEmailException : Exception
{
    public NullOrEmptyEmployeeEmailException() : base("Employee email cannot be null or empty.") {}
    public NullOrEmptyEmployeeEmailException(string message) : base(message) {}
    public NullOrEmptyEmployeeEmailException(string message, Exception inner) : base(message, inner) {}
}