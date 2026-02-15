namespace FinancialBox.Application.Contracts.Services;

public interface ISecretHasherService
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
