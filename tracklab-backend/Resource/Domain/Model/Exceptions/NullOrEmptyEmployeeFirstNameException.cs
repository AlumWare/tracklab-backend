namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyEmployeeFirstNameException : Exception
{
    public NullOrEmptyEmployeeFirstNameException() : base("First name cannot be null or empty!")
    {}
}