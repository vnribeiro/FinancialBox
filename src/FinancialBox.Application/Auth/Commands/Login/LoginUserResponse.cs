namespace FinancialBox.Application.Auth.Commands.Login;

public sealed record LoginUserResponse(string FirstName, string LastName, string Email, string PasswordHash);