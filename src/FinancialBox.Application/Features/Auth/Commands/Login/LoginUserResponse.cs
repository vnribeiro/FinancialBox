namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed record LoginUserResponse(
    Guid Id,
    string FirstName,
    string AccessToken,
    int ExpiresInSeconds);
