using FinancialBox.Application.Abstractions;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Application.Options;
using FinancialBox.Domain.Features.Users.ValueObjects;
using FinancialBox.Domain.Primitives;
using Microsoft.Extensions.Options;

namespace FinancialBox.Application.Features.Auth.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IEmailVerificationCodeRepository emailVerificationCodeRepository,
    ISecureHashService secureHashService,
    IOptions<EmailVerificationOptions> emailVerificationOptions)
    : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly EmailVerificationOptions _emailVerificationOptions = emailVerificationOptions.Value;

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Errors);

        var user = await userRepository.GetByEmailAsync(emailResult.Data.Address, cancellationToken);

        if (user is null)
            return AuthErrors.InvalidOrExpiredCode;

        if (user.IsEmailConfirmed)
            return Result.Success();

        var verificationCode = await emailVerificationCodeRepository.GetMostRecentByUserIdAsync(user.Id, cancellationToken);

        if (verificationCode is null || !verificationCode.CanValidate(DateTime.UtcNow, _emailVerificationOptions.MaxAttempts))
            return AuthErrors.InvalidOrExpiredCode;

        if (!secureHashService.Verify(verificationCode.CodeHash, request.Code))
        {
            verificationCode.RegisterFailedAttempt();
            await unitOfWork.CommitAsync(cancellationToken);
            return AuthErrors.InvalidOrExpiredCode;
        }

        verificationCode.MarkAsUsed(DateTime.UtcNow);
        user.ConfirmEmail();

        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
