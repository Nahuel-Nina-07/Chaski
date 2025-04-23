using Chaski.Infrastructure.Database.Entities.Users;

namespace Chaski.Infrastructure.Database.Seeds;

public static class UserRoleSeed
{
    public static List<UserRoleEntity> GetSeedData()
    {
        return new List<UserRoleEntity>
        {
            new UserRoleEntity
            {
                Id = 1,
                UserId = 1,  // admin
                RoleId = 1,  // Admin
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedByAt = DateTime.UtcNow
            },
            new UserRoleEntity
            {
                Id = 2,
                UserId = 2,  // user
                RoleId = 2,  // User
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedByAt = DateTime.UtcNow
            }
        };
    }
}
