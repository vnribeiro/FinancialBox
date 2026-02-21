using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class EmailVerificationRepository(AppDbContext context)
    : Repository<EmailVerification>(context), IEmailVerificationRepository
{
    private readonly AppDbContext _context = context;

    public Task<EmailVerification?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.EmailVerification
            .Where(code => code.UserId == userId)
            .OrderByDescending(code => code.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
