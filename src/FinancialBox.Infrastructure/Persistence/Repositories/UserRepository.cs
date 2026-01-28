using FinancialBox.Application.Contracts.Persistence;
using FinancialBox.Domain.Users;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : 
    Repository<User>(context), IUserRepository;
