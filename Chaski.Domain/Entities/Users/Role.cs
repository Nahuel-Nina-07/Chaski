using Chaski.Domain.Common;

namespace Chaski.Domain.Entities.Users;

public class Role:BaseEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
}