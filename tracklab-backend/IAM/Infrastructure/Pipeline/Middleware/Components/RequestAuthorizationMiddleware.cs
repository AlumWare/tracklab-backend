using TrackLab.IAM.Application.Internal.OutboundServices;
using TrackLab.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using TrackLab.IAM.Infrastructure.Tokens.JWT.Services;
using TrackLab.IAM.Domain.Repositories;
using TrackLab.Shared.Infrastructure.Multitenancy;

namespace TrackLab.IAM.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
/// RequestAuthorizationMiddleware is a custom middleware.
/// This middleware is used to authorize requests and set tenant context.
/// It validates a token is included in the request header and that the token is valid.
/// If the token is valid then it sets the user in HttpContext.Items["User"] and tenant in TenantContext.
/// </summary>
public class RequestAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public RequestAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// InvokeAsync is called by the ASP.NET Core runtime.
    /// It is used to authorize requests and set tenant context.
    /// It validates a token is included in the request header and that the token is valid.
    /// If the token is valid then it sets the user in HttpContext.Items["User"] and tenant in TenantContext.
    /// </summary>
    public async Task InvokeAsync(
        HttpContext context,
        ITokenService tokenService,
        ITenantContext tenantContext,
        IUserRepository userRepository)
    {
        // skip authorization if endpoint is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.Request.HttpContext.GetEndpoint()?.Metadata
            .Any(m => m.GetType() == typeof(AllowAnonymousAttribute)) ?? false;
        
        if (allowAnonymous)
        {
            // [AllowAnonymous] attribute is set, so skip authorization
            await _next(context);
            return;
        }

        // get token from request header
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        // if token is null then throw exception
        if (token == null) 
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Missing or invalid authorization token");
            return;
        }

        // validate token
        var userId = await tokenService.ValidateToken(token);

        // if token is invalid then throw exception
        if (userId == null) 
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid token");
            return;
        }

        // Extract tenant from token and set in context
        var tenantId = ((TokenService)tokenService).ExtractTenantId(token);
        if (tenantId.HasValue)
        {
            tenantContext.SetTenant(tenantId.Value);
        }

        // get user by id
        var user = await userRepository.FindByIdAsync(userId.Value);
        context.Items["User"] = user;
        
        // call next middleware
        await _next(context);
    }
}