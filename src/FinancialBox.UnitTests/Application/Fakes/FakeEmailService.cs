using FinancialBox.Application.Abstractions.Services;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeEmailService : IEmailService
{
    public readonly List<(string To, string Token)> ConfirmationLinksSent = [];
    public readonly List<(string To, string Token)> PasswordResetsSent = [];

    public Task SendConfirmationLinkAsync(string to, string token, CancellationToken cancellationToken = default)
    {
        ConfirmationLinksSent.Add((to, token));
        return Task.CompletedTask;
    }

    public Task SendPasswordResetAsync(string to, string token, CancellationToken cancellationToken = default)
    {
        PasswordResetsSent.Add((to, token));
        return Task.CompletedTask;
    }
}
