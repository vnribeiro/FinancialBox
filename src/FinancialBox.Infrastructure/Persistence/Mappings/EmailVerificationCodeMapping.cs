using FinancialBox.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class EmailVerificationCodeMapping : IEntityTypeConfiguration<EmailVerificationCode>
{
    public void Configure(EntityTypeBuilder<EmailVerificationCode> builder)
    {
        builder
            .ToTable("EmailVerificationCodes");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.UserId)
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
            .Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt);

        builder
            .HasIndex(x => x.UserId);

        // Relationship to User is FK only — load User separately via IUserRepository when needed.
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
