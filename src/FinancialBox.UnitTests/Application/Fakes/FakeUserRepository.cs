using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.UnitTests.Application.Fakes;

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public void Seed(User user) => _users.Add(user);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task<IEnumerable<User>> GetAllAsync(int pageNumber = 1, int pageSize = 25, CancellationToken cancellationToken = default)
        => Task.FromResult<IEnumerable<User>>(_users);

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public void Update(User entity) { }

    public void Remove(User entity) => _users.Remove(entity);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => Task.FromResult(_users.FirstOrDefault(u => u.Email.Address == email));

    public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => Task.FromResult(_users.Any(u => u.Email.Address == email));
}
