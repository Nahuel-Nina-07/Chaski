using Chaski.Application.Dtos;
using Chaski.Domain.Entities.Users;

namespace Chaski.Application.Extensions.Users;

public static class UserDtoMappings
{
    public static UserDto ToDto(this User user) =>
        new(user.Id, user.Username, user.Email, user.PasswordHash, user.Status);

    public static User ToDomain(this UserDto dto) =>
        new(dto.Id, dto.Username, dto.Email, dto.PasswordHash, dto.Status);
}