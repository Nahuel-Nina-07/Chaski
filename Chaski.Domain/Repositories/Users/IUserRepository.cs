using Chaski.Domain.Entities.Users;
using Chaski.Domain.Repositories.Common;

namespace Chaski.Domain.Repositories.Users;

public interface IUserRepository:IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByEmailConfirmationTokenAsync(string token);
}