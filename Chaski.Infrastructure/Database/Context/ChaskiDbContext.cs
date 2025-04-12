using Chaski.Infrastructure.Database.Entities;
using Chaski.Infrastructure.Database.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Chaski.Infrastructure.Database.Context;

public class ChaskiDbContext:DbContext
{
    // public DbSet<CareerSubjectEnabledEntity> CareerSubjectEnableds { get; set; }
    //Users
    public DbSet<UserEntity> Users { get; set; }
    public ChaskiDbContext(DbContextOptions<ChaskiDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ChaskiDbContext).Assembly);
        base.OnModelCreating(builder);
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