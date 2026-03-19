using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class RoleRepository(AppDbContext context)
    : Repository<Role>(context), IRoleRepository
{
    private readonly AppDbContext _context = context;

    public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _context.Roles
            .FirstOrDefaultAsync(role => role.Name == name, cancellationToken);
    }
}
