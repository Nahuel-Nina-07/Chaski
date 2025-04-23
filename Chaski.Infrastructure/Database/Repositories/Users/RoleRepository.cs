using System.Linq.Expressions;
using Chaski.Domain.Entities.Users;
using Chaski.Domain.Repositories.Users;
using Chaski.Infrastructure.Database.Context;
using Chaski.Infrastructure.Database.Entities.Users;
using Chaski.Infrastructure.Database.Extensions.Users;
using Chaski.Infrastructure.Database.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Chaski.Infrastructure.Database.Repositories.Users;

public class RoleRepository:GenericRepository<RoleEntity>, IRoleRepository
{
    private readonly ChaskiDbContext _context;
    public RoleRepository(ChaskiDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }


    public Task<Role> CreateAsync(Role entity)
    {
        throw new NotImplementedException();
    }

    public Task<Role?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Role> UpdateAsync(Role entity)
    {
        throw new NotImplementedException();
    }

    public Task<List<Role>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Role?> GetByNameAsync(string roleName)
    {
        var entity = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        return entity?.ToDomain();
    }

    public async Task<IList<Role>> GetRolesByUserIdAsync(int userId)
    {
        var roles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(
                _context.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => role
            )
            .ToListAsync();

        return roles.Select(r => r.ToDomain()).ToList();
    }

    public async Task<int?> GetRoleIdByNameAsync(string roleName)
    {
        var role = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == roleName);
        return role?.Id;
    }
}