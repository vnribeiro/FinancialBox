using FinancialBox.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FinancialBox.Infrastructure.Persistence.Interceptors;

internal sealed class AuditInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            Audit(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void Audit(DbContext context)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Property(nameof(BaseEntity.CreatedAt)).CurrentValue = now;

            if (entry.State == EntityState.Modified)
                entry.Property(nameof(BaseEntity.UpdatedAt)).CurrentValue = now;
        }
    }
}
