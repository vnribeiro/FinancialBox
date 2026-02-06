using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Application.Contracts.Services;
using FinancialBox.Domain.Features.User.ValueObjects;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasherService passwordHasher,
    IJwtService jwtService)
    : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
{
    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var user = await userRepository.GetByEmailAsync(email.Address, cancellationToken);

        if (user is null)
        {
            return Result<LoginUserResponse>.Failure(Error.AuthenticationRequired("Invalid email or password."));
        }

        var passwordIsValid = passwordHasher.Verify(user.Password.Hash, request.Password);

        if (!passwordIsValid)
        {
            return Result<LoginUserResponse>.Failure(Error.AuthenticationRequired("Invalid email or password."));
        }

        var token = jwtService.GenerateToken(user);

        var response = new LoginUserResponse(
            token.AccessToken,
            token.ExpiresAtUtc);

        return Result<LoginUserResponse>.Success(response);
    }
}
