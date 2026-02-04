using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Domain.Features.Users;
using FinancialBox.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialBox.Infrastructure.Features.Users;

internal class RoleRepository(AppDbContext context) : IRoleRepository
{
    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await context.Roles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        await context.Roles.AddAsync(role, cancellationToken);
    }
}