using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Domain.Features.FinancialGoal;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal class FinancialGoalRepository(AppDbContext context)
    : Repository<FinancialGoal>(context), IFinancialGoalRepository;

