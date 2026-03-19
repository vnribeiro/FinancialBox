using FinancialBox.Domain.Features.Accounts;

namespace FinancialBox.Application.Abstractions.Repositories;

public interface IRoleRepository: IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
