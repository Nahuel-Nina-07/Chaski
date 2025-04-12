using Chaski.Domain.Common;

namespace Chaski.Domain.Entities.Users;

public class Role:BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}