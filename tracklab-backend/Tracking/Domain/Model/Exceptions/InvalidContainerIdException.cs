namespace Alumware.Tracklab.API.Tracking.Domain.Model.Exceptions;

[Serializable]
public class InvalidContainerIdException : Exception
{
    public InvalidContainerIdException(int containerId) : base("Invalid container id. Value: " + containerId) {}
}