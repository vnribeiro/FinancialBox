using FinancialBox.Application.Abstractions;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Primitives;

namespace FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler(
    IUnitOfWork unitOfWork,
    IAccountRepository accountRepository)
    : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByConfirmationTokenAsync(request.Token, cancellationToken);

        if (account is null)
            return AuthErrors.InvalidOrExpiredToken;

        if (account.IsEmailConfirmed)
            return Result.Success();

        var token = account.EmailConfirmationTokens.First(t => t.Token == request.Token);

        if (!token.CanValidate(DateTime.UtcNow))
            return AuthErrors.InvalidOrExpiredToken;

        token.MarkAsUsed(DateTime.UtcNow);
        account.ConfirmEmail();

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
