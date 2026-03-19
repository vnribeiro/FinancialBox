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
    IHasherService hasherService)
    : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = hasherService.Hash(request.Token);
        var account = await accountRepository.GetByConfirmationTokenHashAsync(tokenHash, cancellationToken);

        if (account is null)
            return AuthErrors.InvalidOrExpiredToken;

        if (account.IsEmailConfirmed)
            return Result.Success();

        var token = account.EmailConfirmationTokens.First(t => t.TokenHash == tokenHash);

        if (!token.CanValidate(DateTime.UtcNow))
            return AuthErrors.InvalidOrExpiredToken;

        token.MarkAsUsed(DateTime.UtcNow);
        account.ConfirmEmail();

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
