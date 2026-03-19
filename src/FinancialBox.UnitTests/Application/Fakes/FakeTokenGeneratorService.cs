using FinancialBox.Application.Abstractions.Services;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeTokenGeneratorService : ITokenGeneratorService
{
    public string GenerateRefreshToken() => "fake_refresh_token";
}
