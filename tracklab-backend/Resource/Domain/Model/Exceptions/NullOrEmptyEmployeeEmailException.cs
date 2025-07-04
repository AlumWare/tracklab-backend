namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyEmployeeEmailException : Exception
{
    public NullOrEmptyEmployeeEmailException() : base("Email cannot be null or empty!") {}
}