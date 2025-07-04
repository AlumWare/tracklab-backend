namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidEmployeePhoneNumberException : Exception
{
    public InvalidEmployeePhoneNumberException(string phoneNumber) : base("Invalid phone number. Value: " + phoneNumber) {}
}