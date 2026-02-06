namespace FinancialBox.Application.Features.Users.Queries.GetMe;

public sealed record GetMeResponse(Guid Id, string FirstName, string LastName, string Email);