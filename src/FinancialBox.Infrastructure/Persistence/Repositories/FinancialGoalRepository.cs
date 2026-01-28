using FinancialBox.Application.Abstractions.Persistence;
using FinancialBox.Domain.FinancialGoals;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

public class FinancialGoalRepository(AppDbContext context)
    : Repository<FinancialGoal>(context), IFinancialGoalRepository;
