using FinancialBox.Domain.Features.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class EmailConfirmationTokenMapping : IEntityTypeConfiguration<EmailConfirmationToken>
{
    public void Configure(EntityTypeBuilder<EmailConfirmationToken> builder)
    {
        builder.ToTable("EmailConfirmationTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountId).IsRequired();

        builder.Property(x => x.TokenHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ExpiresAt).IsRequired();

        builder.Property(x => x.UsedAt);

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.HasIndex(x => x.TokenHash).IsUnique();

        builder.HasIndex(x => x.AccountId);

        builder.HasOne<Account>()
            .WithMany(a => a.EmailConfirmationTokens)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
