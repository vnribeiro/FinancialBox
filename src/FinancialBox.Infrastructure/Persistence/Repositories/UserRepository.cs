using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Domain.Features.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(AppDbContext context) :
    Repository<User>(context), IUserRepository
{
    private readonly AppDbContext _context = context;

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Set<User>()
            .FirstOrDefaultAsync(user => user.Email.Address == email, cancellationToken);
    }
}
