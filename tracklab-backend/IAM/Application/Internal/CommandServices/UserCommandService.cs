using TrackLab.IAM.Application.Internal.OutboundServices;
using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Domain.Model.Commands;
using TrackLab.IAM.Domain.Model.ValueObjects;
using TrackLab.IAM.Domain.Repositories;
using TrackLab.IAM.Domain.Services;
using TrackLab.Shared.Domain.ValueObjects;
using TrackLab.Shared.Infrastructure.Multitenancy;

namespace TrackLab.IAM.Application.Internal.CommandServices;

/// <summary>
/// Implementation of user command service
/// </summary>
public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IHashingService _hashingService;
    private readonly ITokenService _tokenService;
    private readonly ITenantContext _tenantContext;

    public UserCommandService(
        IUserRepository userRepository,
        ITenantRepository tenantRepository,
        IHashingService hashingService,
        ITokenService tokenService,
        ITenantContext tenantContext)
    {
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
        _hashingService = hashingService;
        _tokenService = tokenService;
        _tenantContext = tenantContext;
    }

    public async Task<(Tenant tenant, User user)> Handle(SignUpCommand command)
    {
        // Validate username is not taken (across all tenants)
        var existingUser = await _userRepository.FindByUsernameAsync(command.Username);
        if (existingUser != null)
            throw new Exception("Username is already taken");

        // Create tenant
        var tenant = new Tenant(
            command.Ruc,
            command.LegalName,
            command.CommercialName,
            command.Address,
            command.City,
            command.Country ?? "Peru",
            command.TenantPhone != null ? new PhoneNumber(command.TenantPhone) : null,
            command.TenantEmail != null ? new TrackLab.Shared.Domain.ValueObjects.Email(command.TenantEmail) : null,
            command.Website
        );

        // Save tenant first to get ID
        await _tenantRepository.AddAsync(tenant);
        
        // Save changes to get the generated ID
        await _tenantRepository.SaveChangesAsync();
        
        // Ensure we have the tenant ID from the database
        if (tenant.Id <= 0)
            throw new Exception("Failed to create tenant - ID not generated");

        // Hash password
        var passwordHash = _hashingService.HashPassword(command.Password);

        // Determine roles based on tenant type
        var roles = GetRolesForTenantType(command.TenantType ?? "LOGISTIC");

        // Create admin user with the generated tenant ID
        var user = new User(
            command.Username,
            passwordHash,
            new TrackLab.Shared.Domain.ValueObjects.Email(command.Email),
            command.FirstName,
            command.LastName,
            tenant.Id,
            roles
        );

        // Save user
        await _userRepository.SaveAsync(user);
        
        // Save changes to persist the user
        await _userRepository.SaveChangesAsync();

        return (tenant, user);
    }

    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        // Find user by username (cross-tenant)
        var user = await _userRepository.FindByUsernameAsync(command.Username);
        if (user == null)
            throw new Exception("Invalid credentials");

        // Verify password
        if (!_hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        // Check if user is active
        if (!user.IsActive())
            throw new Exception("User account is deactivated");

        // Generate token
        var token = _tokenService.GenerateToken(user);

        return (user, token);
    }

    public async Task<User> Handle(CreateUserCommand command)
    {
        // Get current tenant from context
        var currentTenantId = _tenantContext.CurrentTenantId;
        if (currentTenantId == null)
            throw new Exception("No tenant context available");

        // Validate username is not taken (across all tenants)
        var existingUser = await _userRepository.FindByUsernameAsync(command.Username);
        if (existingUser != null)
            throw new Exception("Username is already taken");

        // Hash password
        var passwordHash = _hashingService.HashPassword(command.Password);

        // Convert role names to Role objects
        var roles = command.Roles.Select(Role.FromName);

        // Create user
        var user = new User(
            command.Username,
            passwordHash,
            new TrackLab.Shared.Domain.ValueObjects.Email(command.Email),
            command.FirstName,
            command.LastName,
            currentTenantId.Value,
            roles
        );

        // Save user
        await _userRepository.SaveAsync(user);
        
        // Save changes to persist the user
        await _userRepository.SaveChangesAsync();

        return user;
    }

    private static IEnumerable<Role> GetRolesForTenantType(string tenantType)
    {
        return tenantType.ToUpper() switch
        {
            "CLIENT" => [Role.Client],
            "PROVIDER" => [Role.Provider],
            "LOGISTIC" or _ => [Role.Admin] // Default to admin for logistic companies
        };
    }
} 