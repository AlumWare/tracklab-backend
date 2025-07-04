namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidEmployeePositionIdException : Exception
{
    public InvalidEmployeePositionIdException(string positionId) : base("Invalid position id. Value: " + positionId) {}
}