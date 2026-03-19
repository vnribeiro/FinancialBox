using FinancialBox.Domain.Primitives;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Users.Errors;

namespace FinancialBox.Application.Features.Users.Queries.GetMe;

public sealed class GetMeQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetMeQuery, Result<GetMeResponse>>
{
    public async Task<Result<GetMeResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            return UserErrors.NotFound;

        var response = new GetMeResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email.Address);

        return Result<GetMeResponse>.Success(response);
    }
}
