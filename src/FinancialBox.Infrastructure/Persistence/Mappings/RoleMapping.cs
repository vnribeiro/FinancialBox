using FinancialBox.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .ToTable("Roles");

        builder
            .HasKey(r => r.Id);

        builder
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .HasIndex(r => r.Name)
            .IsUnique();

        builder
            .Property(r => r.CreatedAt)
            .IsRequired();

        builder
            .Property(r => r.UpdatedAt);
            
        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new { Id = Guid.Parse("d9aa09b9-0a41-4f9d-8c6b-6f4f3df7a6f9"), Name = "Admin", CreatedAt = seedDate, UpdatedAt = (DateTime?)null },
            new { Id = Guid.Parse("7d2b9c56-1a2d-4c1e-9a62-9e2b7c1f2d0e"), Name = "User", CreatedAt = seedDate, UpdatedAt = (DateTime?)null }
        );
    }
}
