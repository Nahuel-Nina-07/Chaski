using Chaski.Application.Dtos.Users;
using Chaski.Domain.Entities.Users;
using Chaski.Domain.Enums;

namespace Chaski.Application.Extensions.Users;

public static class UserDtoMappings
{
    public static UserDto ToDto(this User user) =>
        new(
            user.Id, 
            user.Username, 
            user.Email, 
            "",
            user.Status
        );

    public static User ToDomain(this CreateUserDto dto) =>
        new(
            0, // ID temporal
            dto.Username,
            dto.Email,
            dto.PasswordHash,
            UserStatus.PendingEmailConfirmation
        );
}