using Chaski.Domain.Common;

namespace Chaski.Domain.Entities.Users;

public class RefreshToken:BaseEntity
{
    public string Token { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public int UserId { get; private set; } 
    
    public RefreshToken(int id, string token, DateTime createdAt, DateTime expiresAt, int userId)
        : base(id)
    {
        Token = token;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        IsRevoked = false;
        UserId = userId;
    }

    public void Revoke()
    {
        IsRevoked = true;
        MarkAsUpdated();
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}