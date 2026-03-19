namespace FinancialBox.Application.Abstractions.Services;

public sealed record JwtToken(string AccessToken, DateTime ExpiresAtUtc);
