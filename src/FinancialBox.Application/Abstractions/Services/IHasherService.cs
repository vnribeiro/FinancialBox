namespace FinancialBox.Application.Abstractions.Services;

public interface IHasherService
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
