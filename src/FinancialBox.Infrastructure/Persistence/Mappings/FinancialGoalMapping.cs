using FinancialBox.Domain.Features.FinancialGoals;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class FinancialGoalMapping : IEntityTypeConfiguration<FinancialGoal>
{
    public void Configure(EntityTypeBuilder<FinancialGoal> builder)
    {
        builder
            .ToTable("FinancialGoals");

        builder
            .HasKey(g => g.Id);

        builder
            .Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder
            .Property(g => g.TargetAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder
            .Property(g => g.IdealMonthlyContribution)
            .HasColumnType("decimal(18,2)");

        builder
            .Property(g => g.CoverImagePath)
            .HasMaxLength(500);

        builder
            .Property(g => g.CreatedAt)
            .IsRequired();

        builder
            .Property(g => g.UpdatedAt);

        builder
            .Property(g => g.Status)
            .IsRequired();

        builder
            .Property(g => g.IsDeleted)
            .HasDefaultValue(false);

        // Relationship to User is FK only — load User separately via IUserRepository when needed.
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(g => g.Transactions)
            .WithOne(t => t.FinancialGoal)
            .HasForeignKey(t => t.FinancialGoalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


