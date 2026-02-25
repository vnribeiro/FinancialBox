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

            email.HasIndex(e => e.Address)
                .IsUnique();
        });

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(u => u.Hash)
                .HasColumnName("PasswordHash")
                .IsRequired()
                .HasMaxLength(100); // PBKDF2-SHA256 with current options produces ~90 chars
        });

        // Many-to-many via join table UserRoles.
        // Role does not expose a Users collection — query by role via IUserRepository.GetByRoleAsync.
        builder
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserRoles",
                role => role
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                user => user
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("UserRoles");
                    join.HasKey("UserId", "RoleId");
                    join.HasIndex("RoleId");
                });

        builder
            .Property(u => u.CreatedAt)
            .IsRequired();

        builder
            .Property(u => u.UpdatedAt);
    }
}

