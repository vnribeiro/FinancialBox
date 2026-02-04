using FinancialBox.Domain.Features.Users;
using System;
using System.Collections.Generic;

namespace FinancialBox.Application.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<string>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddRoleAsync(User user, string roleName, CancellationToken cancellationToken = default);
}
