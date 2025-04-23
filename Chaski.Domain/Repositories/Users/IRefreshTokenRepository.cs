using System.Linq.Expressions;
using Chaski.Domain.Entities.Users;
using Chaski.Domain.Repositories.Common;

namespace Chaski.Domain.Repositories.Users;

public interface IRefreshTokenRepository:IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<RefreshToken?> GetActiveByUserIdAsync(int userId);
    Task RevokeAllForUserAsync(int userId, string ipAddress, string reason);
    Task<List<RefreshToken>> GetAllAsync(Expression<Func<RefreshToken, bool>> predicate);
    Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);

}