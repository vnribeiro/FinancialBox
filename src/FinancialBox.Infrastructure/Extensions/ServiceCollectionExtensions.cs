using FinancialBox.Application.Contracts.Persistence;
using FinancialBox.Application.Contracts.Security;
using FinancialBox.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FinancialBox.Infrastructure.Persistence.Repositories;
using FinancialBox.Infrastructure.Services;

namespace FinancialBox.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DatabaseName = "DefaultConnection";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString(DatabaseName)));

        // Register the DbContext with a scoped lifetime
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFinancialGoalRepository, FinancialGoalRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register services
        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

        return services;
    }
}
