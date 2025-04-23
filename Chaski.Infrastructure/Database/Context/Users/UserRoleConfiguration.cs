using Chaski.Infrastructure.Database.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chaski.Infrastructure.Database.Context.Users;

public class UserRoleConfiguration: IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.HasOne(c => c.User)
            .WithMany(c=>c.UserRoles)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.Role)
            .WithMany(c=>c.UserRoles)
            .HasForeignKey(c=> c.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}