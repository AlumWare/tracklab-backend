namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyEmployeeDNIException : Exception
{
    public NullOrEmptyEmployeeDNIException() : base("DNI cannot be null or empty!") {}
}