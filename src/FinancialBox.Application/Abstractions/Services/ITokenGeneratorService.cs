namespace FinancialBox.Application.Abstractions.Services;

public interface ITokenGeneratorService
{
    string GenerateRefreshToken();
}
