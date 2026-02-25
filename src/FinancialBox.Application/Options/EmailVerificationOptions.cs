namespace FinancialBox.Application.Options;

public sealed class EmailVerificationOptions
{
    public const string SectionName = "EmailVerification";

    public int CodeExpirationMinutes { get; set; } = 15;
    public int CooldownSeconds { get; set; } = 60;
    public int MaxSendsPerHour { get; set; } = 5;
    public int MaxAttempts { get; set; } = 5;
}
