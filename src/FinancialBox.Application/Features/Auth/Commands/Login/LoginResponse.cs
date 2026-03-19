namespace FinancialBox.Application.Features.Auth.Commands.Login;

public sealed record LoginResponse(string AccessToken, DateTime ExpiresAtUtc);
