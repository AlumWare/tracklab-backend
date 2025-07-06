using TrackLab.IAM.Domain.Model.ValueObjects;
using TrackLab.Shared.Domain.ValueObjects;

namespace TrackLab.IAM.Interfaces.ACL;

public interface IIamContextFacade
{
    /// <summary>
    /// Gets user emails based on specific criteria
    /// </summary>
    /// <param name="userId">User ID (optional)</param>
    /// <param name="tenantId">Tenant ID (optional)</param>
    /// <param name="role">User role (optional)</param>
    /// <returns>List of email addresses as strings</returns>
    Task<IEnumerable<string>> GetUserEmailsAsync(long? userId = null, long? tenantId = null, string? role = null);
}
