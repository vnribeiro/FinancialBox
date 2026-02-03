namespace FinancialBox.Application.Contracts.Security;

public interface IPasswordHasherService
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
