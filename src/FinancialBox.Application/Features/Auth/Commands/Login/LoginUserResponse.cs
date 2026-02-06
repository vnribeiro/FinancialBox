namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed record LoginUserResponse(string AccessToken, DateTime ExpiresAtUtc);
