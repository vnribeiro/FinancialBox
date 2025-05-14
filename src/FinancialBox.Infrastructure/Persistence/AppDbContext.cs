using FinancialBox.Domain.Entities;
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
}
