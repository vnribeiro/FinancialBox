namespace FinancialBox.Infrastructure.Email;

internal interface IEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
