using Microsoft.AspNetCore.Mvc;
using TrackLab.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace TrackLab.IAM.Interfaces.REST;

/// <summary>
/// Health controller for testing authentication
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Public health check endpoint - No authentication required
    /// </summary>
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult PublicHealth()
    {
        return Ok(new
        {
            status = "healthy",
            message = "TrackLab API is running",
            timestamp = DateTime.UtcNow,
            authentication = "not_required"
        });
    }

    /// <summary>
    /// Protected health check endpoint - JWT authentication required
    /// </summary>
    [HttpGet("protected")]
    [Authorize]
    public IActionResult ProtectedHealth()
    {
        // Get current user from HttpContext (set by middleware)
        var currentUser = HttpContext.Items["User"] as Domain.Model.Aggregates.User;
        
        return Ok(new
        {
            status = "healthy",
            message = "TrackLab API is running",
            timestamp = DateTime.UtcNow,
            authentication = "required",
            user = currentUser != null ? new
            {
                id = currentUser.Id,
                username = currentUser.Username,
                tenantId = currentUser.TenantId.Value,
                roles = currentUser.GetRoleNames()
            } : null
        });
    }

    /// <summary>
    /// Admin only endpoint - JWT authentication + Admin role required
    /// </summary>
    [HttpGet("admin")]
    [Authorize]
    public IActionResult AdminHealth()
    {
        // Get current user from HttpContext (set by middleware)
        var currentUser = HttpContext.Items["User"] as Domain.Model.Aggregates.User;
        
        if (currentUser == null)
            return Unauthorized(new { message = "User not found in context" });
            
        if (!currentUser.IsAdmin())
            return Forbid("Admin role required");
        
        return Ok(new
        {
            status = "healthy",
            message = "TrackLab API is running - Admin access",
            timestamp = DateTime.UtcNow,
            authentication = "admin_required",
            user = new
            {
                id = currentUser.Id,
                username = currentUser.Username,
                tenantId = currentUser.TenantId.Value,
                roles = currentUser.GetRoleNames(),
                isAdmin = currentUser.IsAdmin()
            }
        });
    }
} 