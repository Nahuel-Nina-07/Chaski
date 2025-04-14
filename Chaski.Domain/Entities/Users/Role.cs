using Chaski.Domain.Common;

namespace Chaski.Domain.Entities.Users;

public class Role:BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    public Role(int id, string name, string description)
        : base(id)
    {
        Name = name;
        Description = description;
    }
}