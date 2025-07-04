namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidEmployeeDNIException : Exception
{
    public InvalidEmployeeDNIException(string dni) : base("Invalid DNI. Value: " + dni) {}
}