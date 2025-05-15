using FinancialBox.Domain.Entities;
using FinancialBox.Domain.Repositories;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

public class FinancialGoalRepository(AppDbContext context)
    : Repository<FinancialGoal>(context), IFinancialGoalRepository;
