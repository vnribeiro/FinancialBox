using FinancialBox.Application.Common;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Contracts.Repositories;

namespace FinancialBox.Application.Features.User.Queries.GetMe;

public sealed class GetMeQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetMeQuery, Result<GetMeResponse>>
{
    public async Task<Result<GetMeResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return Result<GetMeResponse>.Failure(Error.AuthenticationRequired("Invalid email or password."));
        }

        var response = new GetMeResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email.Address);

        return Result<GetMeResponse>.Success(response);
    }
}
