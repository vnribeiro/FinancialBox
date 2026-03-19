namespace FinancialBox.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendConfirmationLinkAsync(string to, string token, CancellationToken cancellationToken = default);
    Task SendPasswordResetAsync(string to, string token, CancellationToken cancellationToken = default);
}
