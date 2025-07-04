namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyEmployeeLastNameException : Exception
{
    public NullOrEmptyEmployeeLastNameException() : base("Last name cannot be null or empty!") {}
}