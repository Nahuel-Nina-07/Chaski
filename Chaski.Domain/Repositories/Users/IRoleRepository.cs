using Chaski.Domain.Entities.Users;
using Chaski.Domain.Repositories.Common;

namespace Chaski.Domain.Repositories.Users;

public interface IRoleRepository : IGenericRepository<Role> 
{
    Task<Role?> GetByNameAsync(string roleName);
    Task<IList<Role>> GetRolesByUserIdAsync(int userId);
    Task<int?> GetRoleIdByNameAsync(string roleName);
}