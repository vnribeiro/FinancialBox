using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class AccountRepository(AppDbContext context) :
    Repository<Account>(context), IAccountRepository
{
    private readonly AppDbContext _context = context;

    public Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Accounts
            .FirstOrDefaultAsync(a => a.Email.Address == email, cancellationToken);
    }

    public Task<Account?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Accounts
            .Include(a => a.Roles)
            .FirstOrDefaultAsync(a => a.Email.Address == email, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Accounts
            .AnyAsync(a => a.Email.Address == email, cancellationToken);
    }
}
