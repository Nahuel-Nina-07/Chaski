using Chaski.Domain.Common;
using Chaski.Domain.Enums;

namespace Chaski.Domain.Entities.Users;

public class User:BaseEntity
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserStatus Status { get; private set; }

    public User(int id, string username, string email, string passwordHash, UserStatus status)
        : base(id)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Status = status;
    }

    public void UpdateStatus(UserStatus newStatus)
    {
        Status = newStatus;
        MarkAsUpdated();
    }
}