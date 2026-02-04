using FinancialBox.Domain.Features.Users;
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

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(u => u.Address)
                .IsRequired()
                .HasMaxLength(255);

            email.HasIndex(e => e.Address).IsUnique();
        });

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(u => u.Hash)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });

        builder
            .Property(u => u.RoleId)
            .IsRequired();

        builder
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(u => u.CreatedAt)
            .IsRequired();

        builder
            .Property(u => u.UpdatedAt);
    }
}

