using System.Linq.Expressions;

namespace Chaski.Domain.Repositories.Common;

public interface IGenericRepository <TEntity> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> DeleteHardAsync(int id);
    Task<bool> ExistsByIdAsync(int id);
    Task<List<TEntity>> GetAllAsync();
}