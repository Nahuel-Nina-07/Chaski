using Chaski.Domain.Common;

namespace Chaski.Domain.Entities.Users;

public class RefreshToken : BaseEntity
{
    public string Token { get; private set; }
    public DateTime Expires { get; private set; }
    public DateTime Created { get; private set; }
    public string CreatedByIp { get; private set; }
    public DateTime? Revoked { get; private set; }
    public string RevokedByIp { get; private set; }
    public string ReplacedByToken { get; private set; }
    public string ReasonRevoked { get; private set; }
    public int UserId { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    public RefreshToken(int id, string token, DateTime expires, DateTime created, 
        string createdByIp, int userId, DateTime? revoked = null, 
        string revokedByIp = null, string replacedByToken = null, 
        string reasonRevoked = null)
        : base(id)
    {
        Token = token;
        Expires = expires;
        Created = created;
        CreatedByIp = createdByIp;
        Revoked = revoked;
        RevokedByIp = revokedByIp;
        ReplacedByToken = replacedByToken;
        ReasonRevoked = reasonRevoked;
        UserId = userId;
    }

    public void Revoke(string ipAddress, string reason = null, string replacedByToken = null)
    {
        Revoked = DateTime.UtcNow;
        RevokedByIp = ipAddress;
        ReasonRevoked = reason;
        ReplacedByToken = replacedByToken;
        MarkAsUpdated();
    }
}