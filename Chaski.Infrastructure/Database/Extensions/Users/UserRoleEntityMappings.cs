using Chaski.Domain.Entities.Users;
using Chaski.Infrastructure.Database.Entities.Users;

namespace Chaski.Infrastructure.Database.Extensions.Users;

public static class UserRoleEntityMappings
{
    public static UserRoleEntity ToEntity(this UserRole entity)
    {
        return new UserRoleEntity
        {
            Id = entity.Id,
            UserId = entity.UserId,
            RoleId = entity.RoleId
        };
    }
    public static UserRole ToDomain(this UserRoleEntity entity)
    {
        return new UserRole(
            id: entity.Id,
            userId: entity.UserId,
            roleId: entity.RoleId
        );
    }
}