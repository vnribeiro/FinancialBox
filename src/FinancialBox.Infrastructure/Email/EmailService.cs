using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Infrastructure.Email.Templates;

namespace FinancialBox.Infrastructure.Email;

internal sealed class EmailService(IEmailSender sender) : IEmailService
{
    public Task SendConfirmationLinkAsync(string to, string token, CancellationToken cancellationToken = default)
        => sender.SendAsync(EmailTemplates.ConfirmationLink(to, token), cancellationToken);

    public Task SendPasswordResetAsync(string to, string token, CancellationToken cancellationToken = default)
        => sender.SendAsync(EmailTemplates.PasswordReset(to, token), cancellationToken);
}
