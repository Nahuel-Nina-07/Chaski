using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaski.Infrastructure.Database.Entities.Users;
[Table("RefreshToken", Schema="USERS" )]

public class RefreshTokenEntity:BaseEntity
{
    [Required]
    [Column("token")]
    public string Token { get; set; }
    
    [Required]
    [Column("expires")]
    public DateTime Expires { get; set; }
    
    [Required]
    [Column("created")]
    public DateTime Created { get; set; }
    
    [Required]
    [Column("createdByIp")]
    public string CreatedByIp { get; set; }
    
    [Column("revoked")]
    public DateTime? Revoked { get; set; }

    [Column("revokedByIp")]
    public string? RevokedByIp { get; set; }

    [Column("replacedByToken")]
    public string? ReplacedByToken { get; set; }

    [Column("reasonRevoked")]
    public string? ReasonRevoked { get; set; }
    
    [Required]
    [Column("userId")]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; }
}