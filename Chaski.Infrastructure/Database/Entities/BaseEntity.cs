namespace Chaski.Infrastructure.Database.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public DateTime LastModifiedByAt { get; set; }
    public int LastModifiedBy { get; set; }
}