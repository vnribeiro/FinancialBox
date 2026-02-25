using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Infrastructure.Email;

namespace FinancialBox.Infrastructure.Services;

internal sealed class EmailService(IEmailSender sender) : IEmailService
{
    public Task SendVerificationCodeAsync(string to, string code, CancellationToken cancellationToken = default)
        => sender.SendAsync(EmailTemplates.VerificationCode(to, code), cancellationToken);

    public Task SendPasswordResetAsync(string to, string token, CancellationToken cancellationToken = default)
        => sender.SendAsync(EmailTemplates.PasswordReset(to, token), cancellationToken);
}
