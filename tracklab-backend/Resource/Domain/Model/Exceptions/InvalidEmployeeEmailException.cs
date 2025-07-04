namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidEmployeeEmailException : Exception
{
    public InvalidEmployeeEmailException(string email) : base("Invalid email. Value: " + email) {}
}