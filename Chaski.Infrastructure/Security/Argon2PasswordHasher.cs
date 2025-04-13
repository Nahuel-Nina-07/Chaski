using System.Security.Cryptography;
using System.Text;
using Chaski.Domain.Security;
using Konscious.Security.Cryptography;

namespace Chaski.Infrastructure.Security;

public sealed class Argon2PasswordHasher : IPasswordHasher
{
    private const int SaltLength = 16;
    private const int HashLength = 32;
    private const int DegreeOfParallelism = 8;
    private const int MemorySizeKb = 131072;
    private const int Iterations = 6;
    
    private const char HashSeparator = ':';

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        var salt = GenerateRandomSalt();
        var hash = ComputeHash(password, salt);
        
        return FormatHash(salt, hash);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("Hashed password cannot be empty", nameof(hashedPassword));

        if (string.IsNullOrWhiteSpace(providedPassword))
            throw new ArgumentException("Provided password cannot be empty", nameof(providedPassword));

        try
        {
            var (salt, storedHash) = ParseHash(hashedPassword);
            var computedHash = ComputeHash(providedPassword, salt);
            
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
        catch (Exception ex) when (ex is FormatException or ArgumentException)
        {
            throw new InvalidOperationException("Invalid password hash format", ex);
        }
    }

    private static byte[] GenerateRandomSalt()
    {
        var salt = new byte[SaltLength];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }

    private static byte[] ComputeHash(string password, byte[] salt)
    {
        using var hasher = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            MemorySize = MemorySizeKb,
            Iterations = Iterations
        };

        return hasher.GetBytes(HashLength);
    }

    private static string FormatHash(byte[] salt, byte[] hash)
    {
        return $"{Convert.ToBase64String(salt)}{HashSeparator}{Convert.ToBase64String(hash)}";
    }

    private static (byte[] salt, byte[] hash) ParseHash(string hashedPassword)
    {
        var parts = hashedPassword.Split(HashSeparator);
        if (parts.Length != 2)
            throw new FormatException("Invalid hash format");

        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        if (salt.Length != SaltLength || hash.Length != HashLength)
            throw new ArgumentException("Invalid hash or salt length");

        return (salt, hash);
    }
}