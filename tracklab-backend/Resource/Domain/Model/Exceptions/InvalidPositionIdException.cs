namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidPositionIdException : Exception
{
    public InvalidPositionIdException(string positionId) : base("Invalid position id. Value: " + positionId) {}
}