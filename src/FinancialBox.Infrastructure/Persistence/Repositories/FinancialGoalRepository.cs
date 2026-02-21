
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Domain.Features.FinancialGoals;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal class FinancialGoalRepository(AppDbContext context)
    : Repository<FinancialGoal>(context), IFinancialGoalRepository
{
    private readonly AppDbContext _context = context;
}

