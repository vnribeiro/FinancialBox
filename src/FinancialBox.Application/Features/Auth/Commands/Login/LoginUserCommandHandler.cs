using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.Application.Features.Auth.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
{
    public LoginUserCommandHandler() {}

    public  Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var password = Password.FromHash(request.Password);

        var user = new User(request.FirstName, request.LastName, email, password);

        var response = new LoginUserResponse(request.FirstName, request.LastName, request.Email);

        return Task.FromResult(Result<LoginUserResponse>.Success(response));
    }
}
