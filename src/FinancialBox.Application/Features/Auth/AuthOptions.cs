namespace FinancialBox.Application.Features.Auth;

public sealed class AuthOptions
{
    public const string SectionName = "Auth";

    public OtpSettings Otp { get; set; } = new();
    public RefreshTokenSettings RefreshToken { get; set; } = new();

    public sealed class OtpSettings
    {
        public int ExpirationMinutes { get; set; } = 15;
        public int CooldownSeconds { get; set; } = 60;
        public int MaxSendsPerHour { get; set; } = 5;
        public int MaxAttempts { get; set; } = 5;
    }

    public sealed class RefreshTokenSettings
    {
        public int ExpirationDays { get; set; } = 7;
    }
}
