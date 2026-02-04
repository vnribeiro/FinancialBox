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

    public async Task<IReadOnlyCollection<string>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<UserRole>()
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddRoleAsync(User user, string roleName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return;

        var normalizedRoleName = roleName.Trim();

        var role = await _context.Set<Role>()
            .FirstOrDefaultAsync(r => r.Name == normalizedRoleName, cancellationToken);

        if (role is null)
        {
            role = new Role(normalizedRoleName);
            await _context.Set<Role>().AddAsync(role, cancellationToken);
        }

        var alreadyAssigned = await _context.Set<UserRole>()
            .AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id, cancellationToken);

        if (alreadyAssigned)
            return;

        await _context.Set<UserRole>()
            .AddAsync(new UserRole(user.Id, role.Id), cancellationToken);
    }
}
