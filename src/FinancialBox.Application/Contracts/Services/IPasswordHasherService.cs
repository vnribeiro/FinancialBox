namespace FinancialBox.Application.Contracts.Services;

public interface IPasswordHasherService
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
