namespace TrackLab.IAM.Infrastructure.Tokens.JWT.Configuration;

/// <summary>
/// JWT configuration settings
/// </summary>
public class JwtConfiguration
{
    public const string SectionName = "Jwt";
    
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
    public int RefreshTokenExpirationDays { get; set; } = 7;
} 