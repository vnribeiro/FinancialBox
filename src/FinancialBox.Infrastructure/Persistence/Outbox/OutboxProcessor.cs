using System.Text.Json;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Domain.DomainEvents;
using FinancialBox.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialBox.Infrastructure.Persistence.Outbox;

internal sealed class OutboxProcessor(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxProcessor> logger,
    IOptions<OutboxOptions> options) : BackgroundService
{
    private readonly OutboxOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await ProcessAsync(cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(_options.IntervalSeconds), cancellationToken);
        }
    }

    private async Task ProcessAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var messages = await db.OutboxMessages
            .Where(m => m.ProcessedAtUtc == null && m.RetryCount < _options.MaxRetries)
            .OrderBy(m => m.CreatedAtUtc)
            .Take(_options.BatchSize)
            .ToListAsync(cancellationToken);

        if (messages.Count == 0)
            return;

        foreach (var message in messages)
        {
            try
            {
                var type = Type.GetType(message.Type) ?? throw new InvalidOperationException($"Cannot resolve type: {message.Type}");

                var domainEvent = (IDomainEvent)JsonSerializer.Deserialize(message.Payload, type)!;

                await mediator.PublishAsync(domainEvent, cancellationToken);

                message.ProcessedAtUtc = DateTime.UtcNow;
                message.Error = null;
            }
            catch (Exception ex)
            {
                message.RetryCount++;
                message.Error = ex.Message;
                logger.LogError(ex, "Failed to process outbox message {MessageId} of type {Type} (attempt {RetryCount}/{MaxRetries})",
                    message.Id, message.Type, message.RetryCount, _options.MaxRetries);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
