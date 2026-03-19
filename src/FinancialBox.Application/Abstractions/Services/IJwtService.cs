using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.Application.Abstractions.Services;

public interface IJwtService
{
    JwtToken GenerateToken(Account account);
}
