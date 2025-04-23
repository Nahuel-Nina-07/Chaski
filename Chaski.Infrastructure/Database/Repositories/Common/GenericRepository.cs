using System.Linq.Expressions;
using Chaski.Domain.Repositories.Common;
using Chaski.Infrastructure.Database.Context;
using Chaski.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chaski.Infrastructure.Database.Repositories.Common;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    public readonly ChaskiDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    protected GenericRepository(
        ChaskiDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var entityEntry = await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var entityEntry = _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> DeleteHardAsync(int id)
    {
        var del = await _dbSet.FindAsync(id);
        if (del is null) return false;
        _dbSet.Remove(del);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
}