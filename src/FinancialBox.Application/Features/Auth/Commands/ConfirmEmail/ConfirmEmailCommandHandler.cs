using FinancialBox.Application.Abstractions;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Primitives;

namespace FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler(
    IUnitOfWork unitOfWork,
    IAccountRepository accountRepository,
    IEmailConfirmationTokenRepository tokenRepository,
    IHasherService hasherService)
    : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = hasherService.Hash(request.Token);
        var confirmationToken = await tokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (confirmationToken is null || !confirmationToken.CanValidate(DateTime.UtcNow))
            return AuthErrors.InvalidOrExpiredToken;

        var account = await accountRepository.GetByIdAsync(confirmationToken.AccountId, cancellationToken);

        if (account is null)
            return AuthErrors.InvalidOrExpiredToken;

        if (account.IsEmailConfirmed)
            return Result.Success();

        confirmationToken.MarkAsUsed(DateTime.UtcNow);
        account.ConfirmEmail();

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
