namespace FinancialBox.Infrastructure.Options;

public sealed class OutboxOptions
{
    public const string SectionName = "Outbox";

    public int IntervalSeconds { get; set; } = 10;
    public int BatchSize { get; set; } = 20;
    public int MaxRetries { get; set; } = 3;
}
