using FinancialBox.Application.Contracts.Repositories;
using FinancialBox.Application.Contracts.Services;
using FinancialBox.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FinancialBox.Infrastructure.Persistence.Repositories;
using FinancialBox.Infrastructure.Services;
using FinancialBox.Application.Contracts;

namespace FinancialBox.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DatabaseName = "DefaultConnection";
    private const string SecureHash= "SecretHasher";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString(DatabaseName)));

        // Register the DbContext with a scoped lifetime
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailVerificationCodeRepository, EmailVerificationCodeRepository>();
        services.AddScoped<IFinancialGoalRepository, FinancialGoalRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Bind PasswordHashingOptions from configuration
        services.Configure<SecureHashOptions>(configuration.GetSection(SecureHash));
        services.AddSingleton<ISecureHashService, SecureHashService>();

        // Register JwtService with options
        services.AddSingleton<IJwtService, JwtService>();

        return services;
    }
}
