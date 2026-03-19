using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class AccountMapping : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder
            .ToTable("Accounts");

        builder
            .HasKey(a => a.Id);

        builder.OwnsOne(a => a.Email, email =>
        {
            email.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(255);

            email.HasIndex(e => e.Address)
                .IsUnique();
        });

        builder.OwnsOne(a => a.Password, password =>
        {
            password.Property(p => p.Hash)
                .HasColumnName("PasswordHash")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder
            .Property(a => a.IsEmailConfirmed)
            .IsRequired()
            .HasDefaultValue(false);

        // Many-to-many via join table AccountRoles.
        // Role does not expose an Accounts collection — query by role via IAccountRepository.GetByRoleAsync.
        builder
            .HasMany(a => a.Roles)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "AccountRoles",
                role => role
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                account => account
                    .HasOne<Account>()
                    .WithMany()
                    .HasForeignKey("AccountId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("AccountRoles");
                    join.HasKey("AccountId", "RoleId");
                    join.HasIndex("RoleId");
                });

        // Relationship to User is FK only — load User separately via IUserRepository when needed.
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(a => a.CreatedAt)
            .IsRequired();

        builder
            .Property(a => a.UpdatedAt);
    }
}
