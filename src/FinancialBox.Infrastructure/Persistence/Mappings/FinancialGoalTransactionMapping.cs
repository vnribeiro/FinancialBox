using FinancialBox.Domain.Features.FinancialGoals;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class FinancialGoalTransactionMapping : IEntityTypeConfiguration<FinancialGoalTransactions>
{
    public void Configure(EntityTypeBuilder<FinancialGoalTransactions> builder)
    {
        builder
            .ToTable("FinancialGoalTransactions");

        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder
            .Property(t => t.TransactionDate)
            .IsRequired();

        builder
            .Property(t => t.Type)
            .IsRequired();

        builder
            .Property(t => t.IsDeleted)
            .HasDefaultValue(false);

        builder
            .Property(t => t.CreatedAt)
            .IsRequired();

        builder
            .Property(t => t.UpdatedAt);
    }
}

