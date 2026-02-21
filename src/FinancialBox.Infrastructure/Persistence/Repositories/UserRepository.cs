using FinancialBox.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(AppDbContext context) :
    Repository<User>(context), IUserRepository
{
    private readonly AppDbContext _context = context;

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .FirstOrDefaultAsync(user => user.Email.Address == email, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .AnyAsync(user => user.Email.Address == email, cancellationToken);
    }
}
