namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class NullOrEmptyPositionNameException : Exception
{
    public  NullOrEmptyPositionNameException() : base("Position name cannot be null or empty.") { }
}