using FinancialBox.Application.Abstractions;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Infrastructure.Options;
using FinancialBox.Infrastructure.Persistence;
using FinancialBox.Infrastructure.Persistence.Interceptors;
using FinancialBox.Infrastructure.Persistence.Outbox;
using FinancialBox.Infrastructure.Persistence.Repositories;
using FinancialBox.Infrastructure.Services;
using FinancialBox.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialBox.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DatabaseName = "DefaultConnection";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AuditInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlite(configuration.GetConnectionString(DatabaseName));
            options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
        });

        // Register repositories and unit of work
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailVerificationCodeRepository, EmailVerificationCodeRepository>();
        services.AddScoped<IFinancialGoalRepository, FinancialGoalRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register options
        services.AddOptions<SecureHashOptions>()
                .BindConfiguration(SecureHashOptions.SectionName)
                .Validate(o => o.Iterations > 0, "SecureHash:Iterations must be greater than zero.")
                .Validate(o => o.SaltSize > 0, "SecureHash:SaltSize must be greater than zero.")
                .Validate(o => o.SubkeySize > 0, "SecureHash:SubkeySize must be greater than zero.")
                .ValidateOnStart();

        services.AddOptions<SmtpOptions>()
            .BindConfiguration(SmtpOptions.SectionName);

        // Register services
        services.AddSingleton<ISecureHashService, SecureHashService>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddScoped<IEmailSender, MailKitEmailSender>();
        services.AddScoped<IEmailService, EmailService>();

        // Register Outbox pattern services
        services.AddOptions<OutboxOptions>()
            .BindConfiguration(OutboxOptions.SectionName);
        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}
