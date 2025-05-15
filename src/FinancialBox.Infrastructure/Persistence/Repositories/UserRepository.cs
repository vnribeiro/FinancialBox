using FinancialBox.Domain.Entities;
using FinancialBox.Domain.Repositories;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : 
    Repository<User>(context), IUserRepository;
