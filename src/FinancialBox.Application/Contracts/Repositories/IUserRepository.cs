using FinancialBox.Domain.Features.Users;
using System;
using System.Collections.Generic;

namespace FinancialBox.Application.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
