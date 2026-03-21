using FinancialBox.Domain.Primitives;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Application.Features.Auth.Errors;
using FinancialBox.Domain.Features.Accounts.ValueObjects;
using FinancialBox.Domain.Features.Accounts;
using FinancialBox.Application.Abstractions;
using Microsoft.Extensions.Options;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUnitOfWork unitOfWork,
    IAccountRepository accountRepository,
    IJwtService jwtService,
    IHasherService hasherService,
    ITokenGeneratorService tokenGeneratorService,
    IOptions<AuthOptions> options)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly AuthOptions _authOptions = options.Value;

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
            return Result<LoginResponse>.Failure(emailResult.Errors);

        var account = await accountRepository.GetByEmailWithRolesAsync(emailResult.Data.Address, cancellationToken);

        if (account is null)
            return AuthErrors.InvalidCredentials;

        var passwordIsValid = hasherService.Verify(account.Password.Hash, request.Password);

        if (!passwordIsValid)
            return AuthErrors.InvalidCredentials;

        if (!account.IsEmailConfirmed)
            return AuthErrors.EmailNotConfirmed;

        var jwtToken = jwtService.GenerateToken(account);

        var base64Token = tokenGeneratorService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(_authOptions.RefreshToken.ExpirationDays);
        var refreshToken = RefreshToken.Create(account.Id, base64Token, expiresAt);
        account.AddRefreshToken(refreshToken);

        accountRepository.Update(account);
        await unitOfWork.CommitAsync(cancellationToken);

        var response = new LoginResponse(
            jwtToken.AccessToken,
            refreshToken.Token,
            jwtToken.ExpiresAtUtc);

        return Result<LoginResponse>.Success(response);
    }
}