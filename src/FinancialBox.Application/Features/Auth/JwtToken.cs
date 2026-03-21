namespace FinancialBox.Application.Features.Auth;

public sealed record JwtToken(string AccessToken, DateTime ExpiresAtUtc);
