using TrackLab.IAM.Interfaces.ACL;

namespace TrackLab.Notifications.Application.Internal.OutboundServices.ACL;

/// <summary>
/// External service to interact with IAM context for notification purposes
/// </summary>
public class ExternalIamService
{
    private readonly IIamContextFacade _iamContextFacade;

    public ExternalIamService(IIamContextFacade iamContextFacade)
    {
        _iamContextFacade = iamContextFacade;
    }

    /// <summary>
    /// Get user emails by specific criteria for notification purposes
    /// </summary>
    /// <param name="userId">Specific user ID</param>
    /// <param name="tenantId">Tenant ID to filter users</param>
    /// <param name="role">Role to filter users</param>
    /// <returns>List of email addresses</returns>
    public async Task<IEnumerable<string>> GetUserEmailsAsync(long? userId = null, long? tenantId = null, string? role = null)
    {
        return await _iamContextFacade.GetUserEmailsAsync(userId, tenantId, role);
    }


    /// <summary>
    /// Get email for a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User email address or null if not found</returns>
    public async Task<string?> GetUserEmailAsync(long userId)
    {
        var emails = await _iamContextFacade.GetUserEmailsAsync(userId, null, null);
        return emails.FirstOrDefault();
    }

    /// <summary>
    /// Get all user emails in a specific tenant
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>List of all user email addresses in the tenant</returns>
    public async Task<IEnumerable<string>> GetAllTenantUserEmailsAsync(long tenantId)
    {
        return await _iamContextFacade.GetUserEmailsAsync(null, tenantId, null);
    }
}
