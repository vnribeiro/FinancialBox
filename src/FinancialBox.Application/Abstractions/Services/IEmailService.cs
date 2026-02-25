namespace FinancialBox.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendVerificationCodeAsync(string to, string code, CancellationToken cancellationToken = default);
    Task SendPasswordResetAsync(string to, string token, CancellationToken cancellationToken = default);
}
