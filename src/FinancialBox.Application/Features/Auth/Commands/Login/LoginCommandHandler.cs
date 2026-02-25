using FinancialBox.Domain.Primitives;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Domain.Features.Users.ValueObjects;
using FinancialBox.Application.Features.Auth.Errors;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    ISecureHashService secretHasherService,
    IJwtService jwtService)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
            return Result<LoginResponse>.Failure(emailResult.Errors);

        var user = await userRepository.GetByEmailAsync(emailResult.Data.Address, cancellationToken);

        if (user is null)
            return AuthErrors.InvalidCredentials;

        var passwordIsValid = secretHasherService.Verify(user.Password.Hash, request.Password);

        if (!passwordIsValid)
            return AuthErrors.InvalidCredentials;

        if (!user.IsEmailConfirmed)
            return AuthErrors.EmailNotConfirmed;

        var token = jwtService.GenerateToken(user);

        var response = new LoginResponse(
            token.AccessToken,
            token.ExpiresAtUtc);

        return Result<LoginResponse>.Success(response);
    }
}
