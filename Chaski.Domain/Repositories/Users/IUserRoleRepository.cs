namespace Chaski.Domain.Repositories.Users;

public interface IUserRoleRepository
{
    Task AssignRoleAsync(int userId, int roleId);
    Task RemoveRoleAsync(int userId, int roleId);
    Task<bool> HasRoleAsync(int userId, string roleName);
}