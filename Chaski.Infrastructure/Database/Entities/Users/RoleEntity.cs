using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaski.Infrastructure.Database.Entities.Users;
[Table("Role", Schema="USERS" )]

public class RoleEntity:BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; }
    
    [Required]
    [Column("description")]
    public string Description { get; set; }
    
    public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
}