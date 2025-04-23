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
    public int Status { get; set; }
    
    [Column("emailConfirmationToken")]
    public string? EmailConfirmationToken { get; set; }

    [Column("isEmailConfirmed")]
    public bool IsEmailConfirmed { get; set; }

    [Column("emailConfirmationTokenExpiry")]
    public DateTime? EmailConfirmationTokenExpiry { get; set; }
    
    [Column("passwordResetTokenHash")]
    public string? PasswordResetTokenHash { get; set; }

    [Column("passwordResetTokenExpiry")]
    public DateTime? PasswordResetTokenExpiry { get; set; }
    
    public virtual ICollection<UserRoleEntity> UserRoles { get; set; } 
    
    public virtual ICollection<RefreshTokenEntity> RefreshTokens { get; set; }
}