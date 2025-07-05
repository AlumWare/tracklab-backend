namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidEmployeeDNIException : Exception
{
    public InvalidEmployeeDNIException() : base("Invalid employee DNI.") {}
    public InvalidEmployeeDNIException(string message) : base(message) {}
    public InvalidEmployeeDNIException(string message, Exception inner) : base(message, inner) {}
}