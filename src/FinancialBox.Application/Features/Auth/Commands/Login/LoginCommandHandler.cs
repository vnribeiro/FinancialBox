using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Application.Contracts.Services;
using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasherService passwordHasher,
    IJwtService jwtService)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var user = await userRepository.GetByEmailAsync(email.Address, cancellationToken);

        if (user is null)
        {
            return Result<LoginResponse>.Failure(Error.AuthenticationRequired("Invalid email or password."));
        }

        var passwordIsValid = passwordHasher.Verify(user.Password.Hash, request.Password);

        if (!passwordIsValid)
        {
            return Result<LoginResponse>.Failure(Error.AuthenticationRequired("Invalid email or password."));
        }

        var token = jwtService.GenerateToken(user);

        var response = new LoginResponse(
            token.AccessToken,
            token.ExpiresAtUtc);

        return Result<LoginResponse>.Success(response);
    }
}
