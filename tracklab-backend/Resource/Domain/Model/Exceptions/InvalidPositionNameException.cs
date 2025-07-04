namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidPositionNameException : Exception
{
    public InvalidPositionNameException(string name) : base("Invalid position name. Value: " + name) { }
}