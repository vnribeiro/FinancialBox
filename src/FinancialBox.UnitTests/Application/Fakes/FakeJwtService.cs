using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeJwtService : IJwtService
{
    public JwtToken GenerateToken(Account account)
        => new("fake_token", DateTime.UtcNow.AddHours(1));
}
