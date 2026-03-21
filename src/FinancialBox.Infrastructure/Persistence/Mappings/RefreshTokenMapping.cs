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
            .HasIndex(x => x.AccountId);

        builder
            .Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .HasIndex(x => x.Token)
            .IsUnique();

        builder
            .Property(x => x.ExpiresAt)
            .IsRequired();

        builder
            .Property(x => x.RevokedAt);

        builder
            .HasOne<Account>()
            .WithMany(a => a.RefreshTokens)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);
    }
}
