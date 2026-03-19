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
    IUserRepository userRepository,
    IEmailVerificationCodeRepository emailVerificationCodeRepository,
    ISecureHashService secureHashService,
    IEmailService emailService,
    IOptions<OtpOptions> emailVerificationOptions)
    : IRequestHandler<ResendConfirmationCommand, Result>
{
    private readonly OtpOptions _emailVerificationOptions = emailVerificationOptions.Value;

    public async Task<Result> Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Errors);

        var user = await userRepository.GetByEmailAsync(emailResult.Data.Address, cancellationToken);

        if (user is null || user.IsEmailConfirmed)
            return Result.Success();

        var utcNow = DateTime.UtcNow;

        var latestCode = await emailVerificationCodeRepository.GetMostRecentByUserIdAsync(user.Id, cancellationToken);

        if (latestCode is not null && latestCode.CreatedAt >= utcNow.AddSeconds(-_emailVerificationOptions.CooldownSeconds))
            return AuthErrors.ResendLimitReached;

        var countLastHour = await emailVerificationCodeRepository.CountSentByUserIdAfterAsync(user.Id, utcNow.AddHours(-1), cancellationToken);

        if (countLastHour >= _emailVerificationOptions.MaxSendsPerHour)
            return AuthErrors.ResendLimitReached;

        var plainCode = OtpGenerator.Generate();
        var codeHash = secureHashService.Hash(plainCode);
        var expiresAt = DateTime.UtcNow.AddMinutes(_emailVerificationOptions.CodeExpirationMinutes);

        var emailVerificationCode = Opt.Create(user.Id, codeHash, expiresAt);
        await emailVerificationCodeRepository.AddAsync(emailVerificationCode, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        await emailService.SendVerificationCodeAsync(user.Email.Address, plainCode, cancellationToken);

        return Result.Success();
    }
}