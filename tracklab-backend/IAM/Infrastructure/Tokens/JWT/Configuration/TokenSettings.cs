namespace TrackLab.IAM.Infrastructure.Tokens.JWT.Configuration;

/// <summary>
/// JWT configuration settings
/// </summary>
public class TokenSettings
{
    public const string SectionName = "TokenSettings";
    
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; } = 1440; // 24 hours
}