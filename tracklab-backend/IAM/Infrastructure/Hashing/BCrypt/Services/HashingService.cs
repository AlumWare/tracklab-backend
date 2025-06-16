using TrackLab.IAM.Application.Internal.OutboundServices;
using BCryptNet = BCrypt.Net.BCrypt;

namespace TrackLab.IAM.Infrastructure.Hashing.BCrypt.Services;

/// <summary>
/// This class is responsible for hashing and validating passwords using BCrypt
/// </summary>
public class HashingService : IHashingService
{
    /// <summary>
    /// This method hashes a password using BCrypt
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <returns>The hashed password</returns>
    public string HashPassword(string password)
    {
        return BCryptNet.HashPassword(password);
    }

    /// <summary>
    /// This method validates a password against a hash using BCrypt
    /// </summary>
    /// <param name="password">The password to validate</param>
    /// <param name="passwordHash">The hash to validate against</param>
    /// <returns>True if the password is valid, false otherwise</returns>
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCryptNet.Verify(password, passwordHash);
    }
}