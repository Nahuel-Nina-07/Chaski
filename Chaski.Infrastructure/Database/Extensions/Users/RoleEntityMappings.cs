using Chaski.Domain.Entities.Users;
using Chaski.Infrastructure.Database.Entities.Users;

namespace Chaski.Infrastructure.Database.Extensions.Users;

public static class RoleEntityMappings
{
    public static RoleEntity ToEntity(this Role entity)
    {
        return new RoleEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
    
    public static Role ToDomain(this RoleEntity entity)
    {
        return new Role(entity.Id, entity.Name, entity.Description);
    }
}