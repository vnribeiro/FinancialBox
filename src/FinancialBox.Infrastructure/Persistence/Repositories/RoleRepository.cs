using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Domain.Features.Users;

namespace FinancialBox.Infrastructure.Persistence.Repositories;

internal class RoleRepository(AppDbContext context)
    : Repository<Role>(context), IRoleRepository;