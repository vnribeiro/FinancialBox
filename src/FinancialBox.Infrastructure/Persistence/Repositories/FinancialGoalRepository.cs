using FinancialBox.Application.Contracts.Persistence;
using FinancialBox.Domain.Features.FinancialGoals;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal class FinancialGoalRepository(AppDbContext context)
    : Repository<FinancialGoal>(context), IFinancialGoalRepository;
