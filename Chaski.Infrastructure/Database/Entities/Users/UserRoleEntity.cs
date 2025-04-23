using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaski.Infrastructure.Database.Entities.Users;
[Table("UserRole", Schema="USERS" )]

public class UserRoleEntity:BaseEntity
{
    [Required]
    [Column("userId")]
    public int UserId { get; set; }
    
    [Required]
    [Column("roleId")]
    public int RoleId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; }
    
    [ForeignKey(nameof(RoleId))]
    public RoleEntity Role { get; set; }
}