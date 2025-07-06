using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TrackLab.IAM.Domain.Model.Aggregates;
using TrackLab.IAM.Application.Internal.OutboundServices;
using TrackLab.IAM.Infrastructure.Tokens.JWT.Configuration;

namespace TrackLab.IAM.Infrastructure.Tokens.JWT.Services;

/// <summary>
/// JWT token service implementation with multitenancy support
/// </summary>
public class TokenService : ITokenService
{
    private readonly TokenSettings _tokenSettings;
    private readonly ILogger<TokenService> _logger;

    public TokenService(TokenSettings tokenSettings, ILogger<TokenService> logger)
    {
        _tokenSettings = tokenSettings;
        _logger = logger;
    }

    /// <summary>
    /// Generate JWT token for user with tenant and role information
    /// </summary>
    /// <param name="user">The user to generate the token for</param>
    /// <returns>The generated JWT token</returns>
    public string GenerateToken(User user)
    {
        return GenerateToken(user, Enumerable.Empty<Claim>());
    }

    public string GenerateToken(User user, IEnumerable<Claim> additionalClaims)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.GetEmail() ?? ""),
            new("tenant_id", user.TenantId.ToString()), // Key claim for tenant resolution
            new("user_id", user.Id.ToString()),
            new("username", user.Username),
            new("full_name", user.GetFullName()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        // Add roles as claims
        foreach (var roleName in user.GetRoleNames())
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            claims.Add(new Claim("roles", roleName)); // Alternative claim for frontend
        }

        // Add additional claims
        claims.AddRange(additionalClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _tokenSettings.Issuer,
            audience: _tokenSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_tokenSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        _logger.LogDebug("Generated JWT token for user {UserId} in tenant {TenantId}", 
            user.Id, user.TenantId);
        
        return tokenString;
    }

    /// <summary>
    /// Validate JWT token and return user ID
    /// </summary>
    /// <param name="token">The token to validate</param>
    /// <returns>The user id if the token is valid, null otherwise</returns>
    public async Task<int?> ValidateToken(string token)
    {
        // If token is null or empty
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _tokenSettings.Issuer,
                ValidAudience = _tokenSettings.Audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                await Task.CompletedTask; // Avoid CS1998 warning
                return userId;
            }

            await Task.CompletedTask; // Avoid CS1998 warning
            return null;
        }
        catch
        {
            await Task.CompletedTask; // Avoid CS1998 warning
            return null;
        }
    }

    /// <summary>
    /// Extract tenant ID from token
    /// </summary>
    /// <param name="token">The token to extract tenant from</param>
    /// <returns>The tenant ID if found</returns>
    public long? ExtractTenantId(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            
            var tenantClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "tenant_id");
            
            if (tenantClaim != null && long.TryParse(tenantClaim.Value, out var tenantId))
            {
                return tenantId;
            }
        }
        catch
        {
            // Ignore parsing errors
        }
        
        return null;
    }

    public bool IsTokenExpired(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            
            return jsonToken.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            return true; // Consider invalid tokens as expired
        }
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}