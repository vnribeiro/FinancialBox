using FinancialBox.Domain.Common;
using FinancialBox.Domain.Features.FinancialGoals;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence;

internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<EmailVerificationCode> EmailVerificationCodes { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<FinancialGoal> FinancialGoals { get; set; } = null!;
    public DbSet<FinancialGoalTransactions> Transactions { get; set; } = null!;
    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Set all string properties to use varchar(255) by default,
        // unless explicitly configured in entity mappings
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var stringProperties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(string));

            foreach (var property in stringProperties)
            {
                if (property.GetColumnType() is null)
                {
                    var maxLength = property.GetMaxLength();
                    property.SetColumnType(maxLength.HasValue ? $"varchar({maxLength})" : "varchar(255)");
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}

