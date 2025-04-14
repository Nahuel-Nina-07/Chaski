using Chaski.Domain.Common;

namespace Chaski.Domain.Entities.Users;

public class UserRole:BaseEntity
{
    public int UserId { get; private set; }
    public int RoleId { get; private set; }
    
    public UserRole(int id, int userId, int roleId)
        : base(id)
    {
        UserId = userId;
        RoleId = roleId;
    }
}