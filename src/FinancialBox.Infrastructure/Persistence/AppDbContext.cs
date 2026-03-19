using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.FinancialGoals;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Opt> EmailVerificationCodes { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<FinancialGoal> FinancialGoals { get; set; } = null!;
    public DbSet<FinancialGoalTransactions> Transactions { get; set; } = null!;
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Set all string properties to use varchar(255) by default,
        // unless explicitly configured in entity mappings
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var stringProperties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(string));

            foreach (var property in stringProperties)
            {
                if (property.GetColumnType() is not null) continue;

                var maxLength = property.GetMaxLength();
                property.SetColumnType(maxLength.HasValue ? $"varchar({maxLength})" : "varchar(255)");
            }
        }
    }
}

