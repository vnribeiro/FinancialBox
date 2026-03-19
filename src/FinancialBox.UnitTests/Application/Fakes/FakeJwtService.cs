using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeJwtService : IJwtService
{
    public TokenResponse GenerateToken(User user)
        => new("fake_token", DateTime.UtcNow.AddHours(1));
}
