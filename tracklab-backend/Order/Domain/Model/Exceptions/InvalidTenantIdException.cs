namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidTenantIdException : Exception
{
    public InvalidTenantIdException(long tenantId) : base("Invalid tenant id. Id: " + tenantId)
    {}
}