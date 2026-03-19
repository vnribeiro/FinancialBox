using FinancialBox.Domain.Features.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .ToTable("RefreshTokens");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.AccountId)
            .IsRequired();

        builder
            .Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder
            .Property(x => x.ExpiresAt)
            .IsRequired();

        builder
            .Property(x => x.RevokedAt);

        builder
            .HasIndex(x => x.Token)
            .IsUnique();

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
