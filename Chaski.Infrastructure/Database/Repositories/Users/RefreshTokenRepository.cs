using System.Linq.Expressions;
using Chaski.Domain.Entities.Users;
using Chaski.Domain.Repositories.Users;
using Chaski.Infrastructure.Database.Context;
using Chaski.Infrastructure.Database.Entities.Users;
using Chaski.Infrastructure.Database.Extensions.Users;
using Chaski.Infrastructure.Database.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Chaski.Infrastructure.Database.Repositories.Users;

public class RefreshTokenRepository:GenericRepository<RefreshTokenEntity>, IRefreshTokenRepository
{
    private readonly ChaskiDbContext _context;
    public RefreshTokenRepository(ChaskiDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public async Task<RefreshToken> CreateAsync(RefreshToken token)
    {
        var entity = token.ToEntity();
        await _context.RefreshTokens.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.ToDomain();
    }

    public async Task<RefreshToken?> GetByIdAsync(int id)
    {
        var entity = await _context.RefreshTokens.FindAsync(id);
        return entity?.ToDomain();
    }

    public async Task<RefreshToken> UpdateAsync(RefreshToken entity)
    {
        var tokenEntity = entity.ToEntity();
        _context.RefreshTokens.Update(tokenEntity);
        await _context.SaveChangesAsync();
        return tokenEntity.ToDomain();
    }

    public async Task<List<RefreshToken>> GetAllAsync()
    {
        var entities = await _context.RefreshTokens.ToListAsync();
        return entities.Select(x => x.ToDomain()).ToList();
    }
    

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        var entity = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);
        return entity?.ToDomain();
    }

    public async Task<RefreshToken?> GetActiveByUserIdAsync(int userId)
    {
        var entity = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && 
                         rt.Revoked == null && 
                         rt.Expires > DateTime.UtcNow)
            .OrderByDescending(rt => rt.Created)
            .FirstOrDefaultAsync();
        return entity?.ToDomain();
    }

    public async Task RevokeAllForUserAsync(int userId, string ipAddress, string reason)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.Revoked == null)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(int userId)
    {
        var entities = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .Where(rt => rt.Revoked == null)
            .Where(rt => rt.Expires > DateTime.UtcNow)
            .OrderByDescending(rt => rt.Created)
            .ToListAsync();
        
        return entities.Select(e => e.ToDomain()).ToList();
    }

    public async Task<List<RefreshToken>> GetAllAsync(Expression<Func<RefreshToken, bool>> predicate)
    {
        var allEntities = await _context.RefreshTokens.ToListAsync();
        return allEntities
            .Select(e => e.ToDomain())
            .Where(predicate.Compile())
            .ToList();
    }
}