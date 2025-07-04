namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyProductDescriptionException : Exception
{
    public  NullOrEmptyProductDescriptionException() : base("Product description cannot be null or empty.") { }
}