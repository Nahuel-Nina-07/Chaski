using Chaski.Infrastructure.Database.Entities.Users;

namespace Chaski.Infrastructure.Database.Seeds;

public static class RoleSeed
{
    public static List<RoleEntity> GetSeedData()
    {
        return new List<RoleEntity>
        {
            new RoleEntity
            {
                Id = 1,
                Name = "Admin",
                Description = "Administrator role",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedByAt = DateTime.UtcNow
            },
            new RoleEntity
            {
                Id = 2,
                Name = "User",
                Description = "Standard user role",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedByAt = DateTime.UtcNow
            }
        };
    }
}