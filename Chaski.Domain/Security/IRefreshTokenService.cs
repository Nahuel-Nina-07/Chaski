using Chaski.Domain.Entities.Users;

namespace Chaski.Domain.Security;

public interface IRefreshTokenService
{
    Task<RefreshToken> GenerateRefreshTokenAsync(int userId, string ipAddress);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string token, string ipAddress, string reason = null, string replacedByToken = null);
    Task RevokeAllRefreshTokensForUserAsync(int userId, string ipAddress, string reason = null);
    Task<bool> IsRefreshTokenValid(string token, int userId);
    Task RotateRefreshTokenAsync(string currentToken, int userId, string ipAddress, string newToken);
    Task CleanExpiredRefreshTokensAsync();
    Task<List<RefreshToken>> GetActiveRefreshTokensAsync(int userId);
    
}