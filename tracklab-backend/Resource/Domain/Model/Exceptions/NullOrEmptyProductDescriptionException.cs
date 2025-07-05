namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyProductDescriptionException : Exception
{
    public NullOrEmptyProductDescriptionException() : base("Product description cannot be null or empty.") {}
    public NullOrEmptyProductDescriptionException(string message) : base(message) {}
    public NullOrEmptyProductDescriptionException(string message, Exception inner) : base(message, inner) {}
}