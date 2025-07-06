using Microsoft.AspNetCore.Mvc;
using TrackLab.IAM.Domain.Model.Queries;
using TrackLab.IAM.Domain.Services;
using TrackLab.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using TrackLab.IAM.Interfaces.REST.Resources;
using TrackLab.IAM.Interfaces.REST.Transform;
using TrackLab.IAM.Domain.Model.Commands;
using TrackLab.IAM.Domain.Model.Aggregates;

namespace TrackLab.IAM.Interfaces.REST;

/// <summary>
/// Authentication controller for IAM operations
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserCommandService _userCommandService;
    private readonly IUserQueryService _userQueryService;

    public AuthenticationController(
        IUserCommandService userCommandService,
        IUserQueryService userQueryService)
    {
        _userCommandService = userCommandService;
        _userQueryService = userQueryService;
    }

    /// <summary>
    /// Sign up - Register new tenant and admin user
    /// </summary>
    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource resource)
    {
        try
        {
            var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
            var (tenant, user) = await _userCommandService.Handle(command);
            
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userResource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Sign in - Authenticate user and return token
    /// </summary>
    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInResource resource)
    {
        try
        {
            var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var (user, token) = await _userCommandService.Handle(command);
            
            var authenticatedUserResource = AuthenticatedUserResourceFromEntityAssembler
                .ToResourceFromEntity(user, token);
            
            return Ok(authenticatedUserResource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create user - Admin creates user within their tenant
    /// </summary>
    [HttpPost("users")]
    [Authorize]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserResource resource)
    {
        try
        {
            var command = CreateUserCommandFromResourceAssembler.ToCommandFromResource(resource);
            var user = await _userCommandService.Handle(command);
            
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userResource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get user by ID within current tenant
    /// </summary>
    [HttpGet("users/{id:long}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(long id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var user = await _userQueryService.Handle(query);
            
            if (user == null)
                return NotFound(new { message = "User not found" });
            
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return Ok(userResource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all users within current tenant
    /// </summary>
    [HttpGet("users")]
    [Authorize]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var query = new GetAllUsersQuery();
            var users = await _userQueryService.Handle(query);
            
            var userResources = UserResourceFromEntityAssembler.ToResourceFromEntity(users);
            return Ok(userResources);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    [HttpGet("profile")]
    [Authorize]
    public IActionResult GetProfile()
    {
        try
        {
            // Get current user from HttpContext (set by middleware)
            var currentUser = HttpContext.Items["User"] as User;
            
            if (currentUser == null)
                return Unauthorized(new { message = "User not found in context" });
            
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(currentUser);
            return Ok(userResource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 