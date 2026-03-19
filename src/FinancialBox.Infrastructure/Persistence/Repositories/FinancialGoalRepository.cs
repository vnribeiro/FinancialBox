
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.FinancialGoals;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal sealed class FinancialGoalRepository(AppDbContext context)
    : Repository<FinancialGoal>(context), IFinancialGoalRepository;

