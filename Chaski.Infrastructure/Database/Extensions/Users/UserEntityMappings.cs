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
            Status = (int)user.Status,
            EmailConfirmationToken = user.EmailConfirmationToken,
            IsEmailConfirmed = user.IsEmailConfirmed,
            EmailConfirmationTokenExpiry = user.EmailConfirmationTokenExpiry,
            PasswordResetTokenHash = user.PasswordResetTokenHash,
            PasswordResetTokenExpiry = user.PasswordResetTokenExpiry
        };
    }

    public static User ToDomain(this UserEntity entity)
    {
        return new User(
            id: entity.Id,
            username: entity.Username,
            email: entity.Email,
            passwordHash: entity.PasswordHash,
            status: (UserStatus)entity.Status,
            emailConfirmationToken: entity.EmailConfirmationToken,
            isEmailConfirmed: entity.IsEmailConfirmed,
            emailConfirmationTokenExpiry: entity.EmailConfirmationTokenExpiry,
            passwordResetTokenHash: entity.PasswordResetTokenHash,
            passwordResetTokenExpiry: entity.PasswordResetTokenExpiry
        );
    }
}