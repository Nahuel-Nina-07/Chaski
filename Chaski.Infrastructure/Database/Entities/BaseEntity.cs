using System.ComponentModel.DataAnnotations.Schema;

namespace Chaski.Infrastructure.Database.Entities;

public abstract class BaseEntity
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [Column("createdBy")]
    public int CreatedBy { get; set; }
    
    [Column("lastModifiedAt")]
    public DateTime LastModifiedByAt { get; set; }
    
    [Column("lastModifiedBy")]
    public int LastModifiedBy { get; set; }
}