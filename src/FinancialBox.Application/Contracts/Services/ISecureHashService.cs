namespace FinancialBox.Application.Contracts.Services;

public interface ISecureHashService
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
