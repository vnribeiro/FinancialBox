using FluentEmail.Core;
using FinancialBox.Application.Abstractions.Services;
using Microsoft.Extensions.Options;

namespace FinancialBox.Infrastructure.Email;

internal sealed class EmailService(IFluentEmailFactory emailFactory, IOptions<AppOptions> appOptions) : IEmailService
{
    private readonly string _baseUrl = appOptions.Value.BaseUrl;

    public Task SendConfirmationLinkAsync(string to, string token, CancellationToken cancellationToken = default)
        => emailFactory.Create()
            .To(to)
            .Subject("FinancialBox – Confirm your email address")
            .UsingTemplateFromEmbedded(
                "FinancialBox.Infrastructure.Email.Templates.ConfirmationLink.liquid",
                new { confirmation_url = $"{_baseUrl}/api/v1/auth/confirm-email?token={token}" },
                typeof(EmailService).Assembly)
            .SendAsync(cancellationToken);

    public Task SendPasswordResetAsync(string to, string token, CancellationToken cancellationToken = default)
        => emailFactory.Create()
            .To(to)
            .Subject("FinancialBox – Password reset request")
            .UsingTemplateFromEmbedded(
                "FinancialBox.Infrastructure.Email.Templates.PasswordReset.liquid",
                new { token },
                typeof(EmailService).Assembly)
            .SendAsync(cancellationToken);
}
