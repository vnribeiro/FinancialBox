using FinancialBox.Domain.Features.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class OtpMapping : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> builder)
    {
        builder
            .ToTable("Otps");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.AccountId)
            .IsRequired();

        builder
            .Property(x => x.CodeHash)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(x => x.ExpiresAt)
            .IsRequired();

        builder
            .Property(x => x.UsedAt);

        builder
            .Property(x => x.Attempts)
            .IsRequired()
            .HasDefaultValue(0);

        builder
            .HasIndex(x => x.AccountId);

        // Relationship to Account is FK only — load Account separately via IAccountRepository when needed.
        builder
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);
    }
}
