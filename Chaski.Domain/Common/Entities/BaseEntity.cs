namespace Chaski.Domain.Common;

public class BaseEntity
{
    public int Id { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    protected BaseEntity(int id) : this()
    {
        Id = id;
    }

    public void SetId(int id)
    {
        if (Id == 0) Id = id;
    }

    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}