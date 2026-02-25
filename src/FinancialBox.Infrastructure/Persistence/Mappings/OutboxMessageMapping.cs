using FinancialBox.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBox.Infrastructure.Persistence.Mappings;

internal sealed class OutboxMessageMapping : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder
            .ToTable("OutboxMessages");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Payload)
            .IsRequired()
            .HasColumnType("TEXT");

        builder
            .Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder
            .Property(x => x.ProcessedAtUtc);

        builder
            .Property(x => x.RetryCount)
            .HasDefaultValue(0);

        builder
            .Property(x => x.Error)
            .HasMaxLength(2000);

        builder
            .HasIndex(x => x.ProcessedAtUtc);
    }
}
