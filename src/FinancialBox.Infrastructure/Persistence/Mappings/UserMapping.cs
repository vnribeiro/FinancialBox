using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("Users");

        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.AccountId)
            .IsRequired();

        builder
            .HasOne<Account>()
            .WithOne()
            .HasForeignKey<User>(u => u.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(u => u.CreatedAt)
            .IsRequired();

        builder
            .Property(u => u.UpdatedAt);
    }
}

