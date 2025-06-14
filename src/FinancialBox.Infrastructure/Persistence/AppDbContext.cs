using FinancialBox.BuildingBlocks.Primitives;
using FinancialBox.Domain.FinancialGoals;
using FinancialBox.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<FinancialGoal> FinancialGoals { get; set; } = null!;
    public DbSet<FinancialGoalTransactions> Transactions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Set UpdatedAt for modified entities automatically
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var prop = entry.Metadata.FindProperty(nameof(BaseEntity.UpdatedAt));

            if (prop is not null)
            {
                entry.Property(nameof(BaseEntity.UpdatedAt)).CurrentValue = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
