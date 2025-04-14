using Chaski.Domain.Entities.Users;
using Chaski.Domain.Repositories.Users;
using Chaski.Infrastructure.Database.Context;
using Chaski.Infrastructure.Database.Entities.Users;
using Chaski.Infrastructure.Database.Extensions.Users;
using Chaski.Infrastructure.Database.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Chaski.Infrastructure.Database.Repositories.Users;

public class UserRepository:GenericRepository<UserEntity>, IUserRepository
{
    private readonly ChaskiDbContext _context;
    public UserRepository(ChaskiDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public async Task<User> CreateAsync(User entity)
    {
        var userEntity = entity.ToEntity();
        var created = await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
        return created.Entity.ToDomain();    
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        return entity?.ToDomain();
        
    }

    public async Task<User> UpdateAsync(User entity)
    {
        var userEntity = entity.ToEntity();
        _context.Users.Update(userEntity);
        await _context.SaveChangesAsync();
        return userEntity.ToDomain();
    }

    public async Task<List<User>> GetAllAsync()
    {
        var entities = await _context.Users.ToListAsync();
        return entities.Select(x => x.ToDomain()).ToList();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
        return entity?.ToDomain();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        return entity?.ToDomain();
    }

    public async Task<User?> GetByEmailConfirmationTokenAsync(string token)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.EmailConfirmationToken == token);
        return entity?.ToDomain();
    }
}