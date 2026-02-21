using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Infrastructure.Options;
using FinancialBox.Infrastructure.Persistence;
using FinancialBox.Infrastructure.Persistence.Outbox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FinancialBox.Infrastructure.Persistence.Repositories;
using FinancialBox.Infrastructure.Services;
using FinancialBox.Application.Abstractions;

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

        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}
