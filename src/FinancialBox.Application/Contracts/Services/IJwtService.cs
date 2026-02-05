using System.Security.Claims;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Application.Contracts.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}
