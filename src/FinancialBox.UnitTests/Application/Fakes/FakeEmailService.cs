using FinancialBox.Application.Abstractions.Services;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeEmailService : IEmailService
{
    public readonly List<(string To, string Code)> VerificationCodesSent = [];
    public readonly List<(string To, string Token)> PasswordResetsSent = [];

    public Task SendVerificationCodeAsync(string to, string code, CancellationToken cancellationToken = default)
    {
        VerificationCodesSent.Add((to, code));
        return Task.CompletedTask;
    }

    public Task SendPasswordResetAsync(string to, string token, CancellationToken cancellationToken = default)
    {
        PasswordResetsSent.Add((to, token));
        return Task.CompletedTask;
    }
}
