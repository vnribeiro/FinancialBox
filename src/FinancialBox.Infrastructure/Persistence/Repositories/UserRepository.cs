using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(AppDbContext context) :
    Repository<User>(context), IUserRepository;