using Chaski.Domain.Entities.Users;
using Chaski.Domain.Repositories.Users;
using Chaski.Domain.Security;

namespace Chaski.Infrastructure.Security;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly ITokenService _tokenService;

    public RefreshTokenService(
        IRefreshTokenRepository refreshTokenRepo,
        ITokenService tokenService)
    {
        _refreshTokenRepo = refreshTokenRepo;
        _tokenService = tokenService;
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId, string ipAddress)
    {
        var refreshToken = new RefreshToken(
            id: 0,
            token: _tokenService.GenerateRefreshToken(),
            expires: DateTime.UtcNow.AddDays(7),
            created: DateTime.UtcNow,
            createdByIp: ipAddress,
            userId: userId
        );

        return await _refreshTokenRepo.CreateAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _refreshTokenRepo.GetByTokenAsync(token);
    }

    public async Task RevokeRefreshTokenAsync(string token, string ipAddress, string reason = null, string replacedByToken = null)
    {
        var refreshToken = await _refreshTokenRepo.GetByTokenAsync(token);
        if (refreshToken != null && refreshToken.IsActive)
        {
            refreshToken.Revoke(ipAddress, reason, replacedByToken);
            await _refreshTokenRepo.UpdateAsync(refreshToken);
        }
    }

    public async Task RevokeAllRefreshTokensForUserAsync(int userId, string ipAddress, string? reason = null)
    {
        await _refreshTokenRepo.RevokeAllForUserAsync(userId, ipAddress, reason ?? "Revocado por sistema");
    }

    public async Task<bool> IsRefreshTokenValid(string token, int userId)
    {
        var refreshToken = await _refreshTokenRepo.GetByTokenAsync(token);
        return refreshToken != null && 
               refreshToken.UserId == userId && 
               refreshToken.IsActive;
    }
    
    public async Task RotateRefreshTokenAsync(string currentToken, int userId, string ipAddress, string newToken)
    {
        await RevokeRefreshTokenAsync(currentToken, ipAddress, "Rotación de token", newToken);
    }

    public async Task CleanExpiredRefreshTokensAsync()
    {
        var expiredTokens = await _refreshTokenRepo.GetAllAsync(rt => 
            rt.IsExpired || (rt.IsRevoked && rt.Revoked < DateTime.UtcNow.AddDays(-30)));
        
        foreach (var token in expiredTokens)
        {
            await _refreshTokenRepo.DeleteHardAsync(token.Id);
        }
    }

    public async Task<List<RefreshToken>> GetActiveRefreshTokensAsync(int userId)
    {
        return await _refreshTokenRepo.GetAllAsync(rt => 
            rt.UserId == userId && 
            !rt.IsRevoked && 
            !rt.IsExpired);
    }
}