using Chaski.Domain.Common;
using Chaski.Domain.Enums;
using Chaski.Domain.Security;

namespace Chaski.Domain.Entities.Users;

public class User : BaseEntity
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserStatus Status { get; private set; }
    public string EmailConfirmationToken { get; private set; }
    public bool IsEmailConfirmed { get; private set; }
    public DateTime? EmailConfirmationTokenExpiry { get; private set; }
    public string PasswordResetTokenHash { get; private set; } // Token hasheado
    public DateTime? PasswordResetTokenExpiry { get; private set; }

    public User(int id, string username, string email, string passwordHash, UserStatus status,
        string emailConfirmationToken = null, bool isEmailConfirmed = false,
        DateTime? emailConfirmationTokenExpiry = null, string passwordResetTokenHash = null,
        DateTime? passwordResetTokenExpiry = null)
        : base(id)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Status = status;
        EmailConfirmationToken = emailConfirmationToken;
        IsEmailConfirmed = isEmailConfirmed;
        EmailConfirmationTokenExpiry = emailConfirmationTokenExpiry;
        PasswordResetTokenHash = passwordResetTokenHash;
        PasswordResetTokenExpiry = passwordResetTokenExpiry;
    }

    public void GenerateEmailConfirmationToken(string token, DateTime expiryDate)
    {
        EmailConfirmationToken = token;
        EmailConfirmationTokenExpiry = expiryDate;
        MarkAsUpdated();
    }

    public void ConfirmEmail()
    {
        if (Status == UserStatus.PendingEmailConfirmation)
        {
            IsEmailConfirmed = true;
            EmailConfirmationToken = null;
            EmailConfirmationTokenExpiry = null;
            Status = UserStatus.Active;
            MarkAsUpdated();
        }
    }

    public void GeneratePasswordResetToken(string token, DateTime expiryDate, IPasswordHasher hasher)
    {
        PasswordResetTokenHash = hasher.HashPassword(token);
        PasswordResetTokenExpiry = expiryDate;
        MarkAsUpdated();
    }

    public bool VerifyPasswordResetToken(string token, IPasswordHasher hasher)
    {
        return !string.IsNullOrEmpty(PasswordResetTokenHash) &&
               hasher.VerifyPassword(PasswordResetTokenHash, token) &&
               PasswordResetTokenExpiry > DateTime.UtcNow;
    }

    public void ClearPasswordResetToken()
    {
        PasswordResetTokenHash = null;
        PasswordResetTokenExpiry = null;
        MarkAsUpdated();
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        ClearPasswordResetToken();
        MarkAsUpdated();
    }

    public void UpdateStatus(UserStatus newStatus)
    {
        Status = newStatus;
        MarkAsUpdated();
    }
}