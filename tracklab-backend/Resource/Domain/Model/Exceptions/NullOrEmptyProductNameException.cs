namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyProductNameException : Exception
{
    public NullOrEmptyProductNameException() : base("Product name cannot be null or empty.") { }
}