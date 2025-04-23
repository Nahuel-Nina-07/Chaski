using Chaski.Domain.Repositories.Users;
using Chaski.Infrastructure.Database.Context;
using Chaski.Infrastructure.Database.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Chaski.Infrastructure.Database.Repositories.Users;

public class UserRoleRepository:IUserRoleRepository
{
    private readonly ChaskiDbContext _context;

    public UserRoleRepository(ChaskiDbContext context)
    {
        _context = context;
    }
    
    public async Task AssignRoleAsync(int userId, int roleId)
    {
        var userRole = new UserRoleEntity
        {
            UserId = userId,
            RoleId = roleId
        };
        _context.Add(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRoleAsync(int userId, int roleId)
    {
        var userRole = await _context.Set<UserRoleEntity>()
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (userRole != null)
        {
            _context.Remove(userRole);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasRoleAsync(int userId, string roleName)
    {
        return await _context.UserRoles
            .Include(ur => ur.Role)
            .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == roleName);
    }
}