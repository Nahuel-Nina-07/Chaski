using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaski.Infrastructure.Database.Entities.Users;
[Table("User", Schema="USERS" )]

public class UserEntity:BaseEntity
{
    [Required]
    [Column("username")]
    public string Username { get; set; }
    
    [Required]
    [Column("email")]
    public string Email { get; set; }
    
    [Required]
    [Column("passwordHash")]
    public string PasswordHash { get; set; }
    
    [Required]
    [Column("status")]
    public string Status { get; set; }
}