using FinancialBox.Application.Contracts.Security;
using Microsoft.AspNetCore.Identity;

namespace FinancialBox.Infrastructure.Services;

internal sealed class PasswordHasherService : IPasswordHasherService
{
    private readonly IPasswordHasher<object> _hasher = new PasswordHasher<object>();
    private readonly object _user = new();

    public string Hash(string password)
        => _hasher.HashPassword(_user, password);

    public bool Verify(string hash, string password)
        => _hasher.VerifyHashedPassword(_user, hash, password) == PasswordVerificationResult.Success;
}
