using Chaski.Domain.Common;
using Chaski.Domain.Enums;

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

    public User(int id, string username, string email, string passwordHash, UserStatus status,
        string emailConfirmationToken = null, bool isEmailConfirmed = false, 
        DateTime? emailConfirmationTokenExpiry = null)
        : base(id)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Status = status;
        EmailConfirmationToken = emailConfirmationToken;
        IsEmailConfirmed = isEmailConfirmed;
        EmailConfirmationTokenExpiry = emailConfirmationTokenExpiry;
    }

    public void UpdateStatus(UserStatus newStatus)
    {
        Status = newStatus;
        MarkAsUpdated();
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
            Status = UserStatus.Active; // Cambia el estado automáticamente
            MarkAsUpdated();
        }
    }
}