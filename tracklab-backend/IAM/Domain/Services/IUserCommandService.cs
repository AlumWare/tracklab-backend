using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Domain.Model.Commands;

namespace TrackLab.IAM.Domain.Services;

/// <summary>
/// Interface for user command service
/// </summary>
public interface IUserCommandService
{
    /// <summary>
    /// Handle sign up command - creates tenant and admin user
    /// </summary>
    Task<(Tenant tenant, User user)> Handle(SignUpCommand command);
    
    /// <summary>
    /// Handle sign in command - authenticates user and returns token
    /// </summary>
    Task<(User user, string token)> Handle(SignInCommand command);
    
    /// <summary>
    /// Handle create user command - creates user within current tenant
    /// </summary>
    Task<User> Handle(CreateUserCommand command);
}
