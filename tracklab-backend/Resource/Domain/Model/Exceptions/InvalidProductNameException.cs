namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidProductNameException : Exception
{
    public InvalidProductNameException() : base("Invalid product name.") {}
    public InvalidProductNameException(string message) : base(message) {}
    public InvalidProductNameException(string message, Exception inner) : base(message, inner) {}
}