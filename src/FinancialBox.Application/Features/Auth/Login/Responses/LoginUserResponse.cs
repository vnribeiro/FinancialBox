namespace FinancialBox.Application.Features.Auth.Login.Responses;

public sealed record LoginUserResponse(string FirstName, string LastName, string Email, string PasswordHash);

