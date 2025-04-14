using Chaski.Domain.Enums;

namespace Chaski.Application.Dtos.Users;

public record UserDto
(
    int Id,
    string Username,
    string Email,
    string PasswordHash,
    UserStatus Status
);

public record CreateUserDto(
    string Username,
    string Email,
    string PasswordHash
);
