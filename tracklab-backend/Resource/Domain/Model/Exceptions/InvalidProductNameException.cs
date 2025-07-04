namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidProductNameException : Exception
{
    public InvalidProductNameException(string name) : base("Invalid product name. Value: " + name) { }
}