using Chaski.Domain.Enums;

namespace Chaski.Application.Dtos;

public record UserDto
(
    int Id,
    string Username,
    string Email,
    string PasswordHash,
    UserStatus Status
);