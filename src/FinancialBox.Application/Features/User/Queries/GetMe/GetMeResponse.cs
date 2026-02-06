namespace FinancialBox.Application.Features.User.Queries.GetMe;

public sealed record GetMeResponse(Guid Id, string FirstName, string LastName, string Email);

