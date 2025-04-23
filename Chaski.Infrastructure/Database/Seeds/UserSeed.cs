using Chaski.Domain.Enums;
using Chaski.Domain.Security;
using Chaski.Infrastructure.Database.Entities.Users;

namespace Chaski.Infrastructure.Database.Seeds;

public static class UserSeed
{
    public static List<UserEntity> GetSeedData(IPasswordHasher hasher)
    {
        var adminPasswordHash = hasher.HashPassword("Admin123!");
        var userPasswordHash = hasher.HashPassword("User123!");

        return new List<UserEntity>
        {
            new UserEntity
            {
                Id = 1,
                Username = "admin",
                Email = "admin@chaski.com",
                PasswordHash = adminPasswordHash,
                Status = (int)UserStatus.Active,
                IsEmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedByAt = DateTime.UtcNow
            },
            new UserEntity
            {
                Id = 2,
                Username = "user",
                Email = "user@chaski.com",
                PasswordHash = userPasswordHash,
                Status = (int)UserStatus.Active,
                IsEmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedByAt = DateTime.UtcNow
            }
        };
    }
}