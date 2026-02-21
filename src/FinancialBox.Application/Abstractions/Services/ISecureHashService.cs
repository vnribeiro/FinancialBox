namespace FinancialBox.Application.Abstractions.Services;

public interface ISecureHashService
{
    string Hash(string password);
    bool Verify(string hash, string password);
}
