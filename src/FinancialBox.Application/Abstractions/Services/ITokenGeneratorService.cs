namespace FinancialBox.Application.Abstractions.Services;

public interface ITokenGeneratorService
{
    string GenerateOtp(int digits = 6);
    string GenerateRefreshToken();
}
