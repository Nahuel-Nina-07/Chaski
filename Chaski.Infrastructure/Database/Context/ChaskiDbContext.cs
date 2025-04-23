using Chaski.Infrastructure.Database.Entities;
using Chaski.Infrastructure.Database.Entities.Users;
using Chaski.Infrastructure.Database.Seeds;
using Chaski.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Chaski.Infrastructure.Database.Context;

public class ChaskiDbContext:DbContext
{
    // public DbSet<CareerSubjectEnabledEntity> CareerSubjectEnableds { get; set; }
    //Users
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public ChaskiDbContext(DbContextOptions<ChaskiDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ChaskiDbContext).Assembly);
        
        var hasher = new Argon2PasswordHasher();
        builder.Entity<RoleEntity>().HasData(RoleSeed.GetSeedData());
        builder.Entity<UserEntity>().HasData(UserSeed.GetSeedData(hasher));
        builder.Entity<UserRoleEntity>().HasData(UserRoleSeed.GetSeedData());
    }

    public override int SaveChanges()
    {
        ApplyAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditFields()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = GetCurrentUserId();
                    entry.Entity.LastModifiedByAt = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = GetCurrentUserId();
                    break;

                case EntityState.Modified:
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                    entry.Property(nameof(BaseEntity.CreatedBy)).IsModified = false;
                    entry.Entity.LastModifiedByAt = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = GetCurrentUserId();
                    break;
            }
        }
    }

    private int GetCurrentUserId()
    {
        return 123; // En producción, se inyecta un servicio para obtener el usuario actual
    }
}