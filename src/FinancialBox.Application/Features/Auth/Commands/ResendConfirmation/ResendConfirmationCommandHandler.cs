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
    IEmailConfirmationTokenRepository tokenRepository,
    IHasherService hasherService,
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

        var account = await accountRepository.GetByEmailAsync(emailResult.Data.Address, cancellationToken);

        if (account is null || account.IsEmailConfirmed)
            return Result.Success();

        var utcNow = DateTime.UtcNow;

        var latestToken = await tokenRepository.GetMostRecentByAccountIdAsync(account.Id, cancellationToken);

        if (latestToken is not null && latestToken.CreatedAt >= utcNow.AddSeconds(-_options.EmailConfirmation.CooldownSeconds))
            return AuthErrors.ResendLimitReached;

        var countLastHour = await tokenRepository.CountSentByAccountIdAfterAsync(account.Id, utcNow.AddHours(-1), cancellationToken);

        if (countLastHour >= _options.EmailConfirmation.MaxSendsPerHour)
            return AuthErrors.ResendLimitReached;

        var plainToken = Guid.NewGuid().ToString();
        var tokenHash = hasherService.Hash(plainToken);
        var expiresAt = utcNow.AddMinutes(_options.EmailConfirmation.ExpirationMinutes);

        var confirmationToken = EmailConfirmationToken.Create(account.Id, tokenHash, expiresAt);
        await tokenRepository.AddAsync(confirmationToken, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        await emailService.SendConfirmationLinkAsync(account.Email.Address, plainToken, cancellationToken);

        return Result.Success();
    }
}
