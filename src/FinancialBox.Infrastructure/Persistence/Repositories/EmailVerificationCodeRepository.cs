using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class EmailVerificationCodeRepository(AppDbContext context)
    : Repository<EmailVerificationCode>(context), IEmailVerificationCodeRepository
{
    private readonly AppDbContext _context = context;

    public Task<EmailVerificationCode?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.EmailVerificationCodes
            .Where(code => code.UserId == userId)
            .OrderByDescending(code => code.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
