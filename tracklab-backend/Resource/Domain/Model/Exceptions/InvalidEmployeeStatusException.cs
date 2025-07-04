namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidEmployeeStatusException : Exception
{
    public InvalidEmployeeStatusException(string status) : base("Invalid status. Value: " + status) {}
}