using Chaski.Application.Dtos;
using Chaski.Domain.Entities.Users;
using Chaski.Domain.Enums;
using Chaski.Infrastructure.Database.Entities.Users;

namespace Chaski.Infrastructure.Database.Extensions.Users;

public static class UserEntityMappings
{
    public static UserEntity ToEntity(this User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            Status = user.Status.ToString()
        };
    }

    public static User ToDomain(this UserEntity entity)
    {
        return new User
        (
            entity.Id,
            entity.Username,
            entity.Email,
            entity.PasswordHash,
            Enum.Parse<UserStatus>(entity.Status)
        );
    }
}