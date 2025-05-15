using FinancialBox.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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
            .Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(u => u.PasswordHash)
            .IsRequired();

        builder
            .Property(u => u.CreatedAt)
            .IsRequired();

        builder
            .Property(u => u.UpdatedAt);

        builder.HasMany(u => u.FinancialGoals)
            .WithOne(g => g.User)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

