using System.Security.Claims;
using TrackLab.Shared.Infrastructure.Multitenancy;

namespace TrackLab.IAM.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
/// Middleware to extract tenantId from JWT and set it in the TenantContext
/// </summary>
public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantResolutionMiddleware> _logger;

    public TenantResolutionMiddleware(RequestDelegate next, ILogger<TenantResolutionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        try
        {
            // Clear any existing tenant context
            tenantContext.ClearTenant();

            // Try to extract tenant from JWT claims
            var tenantId = ExtractTenantFromJWT(context);
            
            if (tenantId.HasValue)
            {
                tenantContext.SetTenant(tenantId.Value);
                _logger.LogDebug("Tenant {TenantId} set in context for request {Path}", 
                    tenantId.Value, context.Request.Path);
            }
            else
            {
                // Try alternative methods (header, subdomain, etc.)
                tenantId = ExtractTenantFromAlternativeSources(context);
                
                if (tenantId.HasValue)
                {
                    tenantContext.SetTenant(tenantId.Value);
                    _logger.LogDebug("Tenant {TenantId} set from alternative source for request {Path}", 
                        tenantId.Value, context.Request.Path);
                }
                else
                {
                    _logger.LogWarning("No tenant found for request {Path}", context.Request.Path);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resolving tenant for request {Path}", context.Request.Path);
            // Continue without tenant - let authorization handle it
        }

        await _next(context);
    }

    /// <summary>
    /// Extract tenant ID from JWT claims
    /// </summary>
    private long? ExtractTenantFromJWT(HttpContext context)
    {
        var user = context.User;
        
        if (user?.Identity?.IsAuthenticated != true)
            return null;

        // Look for tenant claim in JWT
        var tenantClaim = user.FindFirst("tenant_id") ?? 
                         user.FindFirst("tenantId") ?? 
                         user.FindFirst(ClaimTypes.GroupSid); // Alternative claim type

        if (tenantClaim != null && long.TryParse(tenantClaim.Value, out var tenantId))
        {
            return tenantId;
        }

        return null;
    }

    /// <summary>
    /// Extract tenant ID from alternative sources (headers, subdomain, etc.)
    /// </summary>
    private long? ExtractTenantFromAlternativeSources(HttpContext context)
    {
        // Try from custom header
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValues))
        {
            var headerValue = headerValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(headerValue) && long.TryParse(headerValue, out var tenantId))
            {
                return tenantId;
            }
        }

        // Try from subdomain (e.g., tenant123.domain.com)
        var host = context.Request.Host.Host;
        if (!string.IsNullOrEmpty(host))
        {
            var parts = host.Split('.');
            if (parts.Length > 2) // Has subdomain
            {
                var subdomain = parts[0];
                // Extract tenant ID from subdomain pattern
                if (subdomain.StartsWith("tenant") && subdomain.Length > 6)
                {
                    var tenantPart = subdomain.Substring(6); // Remove "tenant"
                    if (long.TryParse(tenantPart, out var tenantId))
                    {
                        return tenantId;
                    }
                }
            }
        }

        // Try from query parameter (for development/testing)
        if (context.Request.Query.TryGetValue("tenantId", out var queryValues))
        {
            var queryValue = queryValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(queryValue) && long.TryParse(queryValue, out var tenantId))
            {
                return tenantId;
            }
        }

        return null;
    }
} 