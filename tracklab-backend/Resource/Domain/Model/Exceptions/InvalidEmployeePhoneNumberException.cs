namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidEmployeePhoneNumberException : Exception
{
    public InvalidEmployeePhoneNumberException() : base("Invalid employee phone number.") {}
    public InvalidEmployeePhoneNumberException(string message) : base(message) {}
    public InvalidEmployeePhoneNumberException(string message, Exception inner) : base(message, inner) {}
}