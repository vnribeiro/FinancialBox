namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed record LoginUserResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string AccessToken,
    int ExpiresInSeconds);
