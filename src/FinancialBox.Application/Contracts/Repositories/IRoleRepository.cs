using FinancialBox.Domain.Features.User;

namespace FinancialBox.Application.Contracts.Repositories;

public interface IRoleRepository: IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
