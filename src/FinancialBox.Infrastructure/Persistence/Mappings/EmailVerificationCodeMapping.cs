using FinancialBox.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class EmailVerificationMapping : IEntityTypeConfiguration<EmailVerification>
{
    public void Configure(EntityTypeBuilder<EmailVerification> builder)
    {
        builder
            .ToTable("EmailVerificationCodes");

        builder
            .HasKey(code => code.Id);

        builder
            .Property(code => code.UserId)
            .IsRequired();

        builder
            .Property(code => code.CodeHash)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(code => code.ExpiresAt)
            .IsRequired();

        builder
            .Property(code => code.UsedAt);

        builder
            .Property(code => code.Attempts)
            .IsRequired()
            .HasDefaultValue(0);

        builder
            .Property(code => code.CreatedAt)
            .IsRequired();

        builder
            .Property(code => code.UpdatedAt);

        builder
            .HasIndex(code => code.UserId);

        builder
            .HasOne(code => code.User)
            .WithMany()
            .HasForeignKey(code => code.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
