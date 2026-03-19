using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeRoleRepository : IRoleRepository
{
    private readonly List<Role> _roles = [];

    public FakeRoleRepository()
    {
        _roles.Add(new Role(Role.DefaultName));
    }

    public void Seed(Role role) => _roles.Add(role);

    public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => Task.FromResult(_roles.FirstOrDefault(r => r.Name == name));

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_roles.FirstOrDefault(r => r.Id == id));

    public Task<IEnumerable<Role>> GetAllAsync(int pageNumber = 1, int pageSize = 25, CancellationToken cancellationToken = default)
        => Task.FromResult<IEnumerable<Role>>(_roles);

    public Task AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        _roles.Add(entity);
        return Task.CompletedTask;
    }

    public void Update(Role entity) { }

    public void Remove(Role entity) => _roles.Remove(entity);
}
