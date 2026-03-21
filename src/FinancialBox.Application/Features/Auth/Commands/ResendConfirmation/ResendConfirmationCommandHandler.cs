using FinancialBox.Application.Abstractions;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.Domain.Primitives;
using Microsoft.Extensions.Options;

namespace FinancialBox.Application.Features.Auth.Commands.ResendConfirmation;

public sealed class ResendConfirmationCommandHandler(
    IUnitOfWork unitOfWork,
    IAccountRepository accountRepository,
    IEmailService emailService,
    IOptions<AuthOptions> options)
    : IRequestHandler<ResendConfirmationCommand, Result>
{
    private readonly AuthOptions _options = options.Value;

    public async Task<Result> Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Errors);

        var account = await accountRepository.GetByEmailWithConfirmationTokensAsync(emailResult.Data.Address, cancellationToken);

        if (account is null || account.IsEmailConfirmed)
            return Result.Success();

        var utcNow = DateTime.UtcNow;
        var tokens = account.EmailConfirmationTokens;

        var latestToken = tokens.OrderByDescending(t => t.CreatedAt).FirstOrDefault();

        if (latestToken is not null && latestToken.CreatedAt >= utcNow.AddSeconds(-_options.EmailConfirmation.CooldownSeconds))
            return AuthErrors.ResendLimitReached;

        var countLastHour = tokens.Count(t => t.CreatedAt >= utcNow.AddHours(-1));

        if (countLastHour >= _options.EmailConfirmation.MaxSendsPerHour)
            return AuthErrors.ResendLimitReached;

        var expiresAt = utcNow.AddMinutes(_options.EmailConfirmation.ExpirationMinutes);
        var emailConfirmationToken = EmailConfirmationToken.Create(account.Id, expiresAt);
        account.AddEmailConfirmationToken(emailConfirmationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        await emailService.SendConfirmationLinkAsync(account.Email.Address, emailConfirmationToken.Token, cancellationToken);

        return Result.Success();
    }
}
