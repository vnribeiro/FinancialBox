using FinancialBox.Application.Contracts.Persistence;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal class UserRepository(AppDbContext context) : 
    Repository<User>(context), IUserRepository;
