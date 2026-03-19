using System.IdentityModel.Tokens.Jwt;
using FinancialBox.Application.Abstractions;
using FinancialBox.Application.Abstractions.Repositories;
using FinancialBox.Application.Abstractions.Services;
using FinancialBox.Infrastructure.Email;
using FinancialBox.Infrastructure.Persistence;
using FinancialBox.Infrastructure.Persistence.Interceptors;
using FinancialBox.Infrastructure.Persistence.Outbox;
using FinancialBox.Infrastructure.Persistence.Repositories;
using FinancialBox.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using FinancialBox.Infrastructure.Services.Options;

namespace FinancialBox.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DatabaseName = "DefaultConnection";

    /// <summary>
    /// Central entry point to register all infrastructure-related configurations:
    /// - Persistence
    /// - Application Services
    /// - Authentication
    /// - Outbox
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="configuration">The IConfiguration instance.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddApplicationServices()
            .AddEmailServices()
            .AddAuthenticationConfiguration(configuration)
            .AddOutbox();

        return services;
    }

    /// <summary>
    /// Registers the database context, interceptors, repositories, and unit of work.
    /// </summary>
    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register the audit interceptor as a singleton since
        // it is stateless and can be shared across DbContext instances.
        services.AddSingleton<AuditInterceptor>();
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlite(configuration.GetConnectionString(DatabaseName));
            options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
        });

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFinancialGoalRepository, FinancialGoalRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    /// <summary>
    /// Registers core application-level services and configurations.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddOptions<HasherOptions>()
            .BindConfiguration(HasherOptions.SectionName)
            .Validate(o => o.Iterations > 0, "Hasher:Iterations must be greater than zero.")
            .Validate(o => o.SaltSize > 0, "Hasher:SaltSize must be greater than zero.")
            .Validate(o => o.SubkeySize > 0, "Hasher:SubkeySize must be greater than zero.")
            .ValidateOnStart();

        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IHasherService, HasherService>();
        services.AddSingleton<ITokenGeneratorService, TokenGeneratorService>();

        return services;
    }

    /// <summary>
    /// Registers email-related services and validates SMTP configuration on startup.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddEmailServices(this IServiceCollection services)
    {
        services.AddOptions<SmtpOptions>()
            .BindConfiguration(SmtpOptions.SectionName)
            .Validate(o => !string.IsNullOrWhiteSpace(o.Host), "Smtp:Host is required.")
            .Validate(o => o.Port > 0, "Smtp:Port must be greater than zero.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Username), "Smtp:Username is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Password), "Smtp:Password is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.FromAddress), "Smtp:FromAddress is required.")
            .ValidateOnStart();

        services.AddScoped<IEmailSender, MailKitEmailSender>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }

    /// <summary>
    /// Configures JWT bearer authentication with validation parameters bound from application settings.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddAuthenticationConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetRequiredSection(JwtOptions.SectionName);

        services.AddOptions<JwtOptions>()
            .Bind(jwtSection)
            .Validate(o => !string.IsNullOrWhiteSpace(o.Issuer), "Jwt:Issuer is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Audience), "Jwt:Audience is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Key), "Jwt:Key is required.")
            .Validate(o => o.Key.Length >= 32, "Jwt:Key must be at least 256 bits.")
            .Validate(o => o.ExpiresInHours > 0, "Jwt:ExpiresInHours must be greater than zero.")
            .ValidateOnStart();

        var jwtOptions = jwtSection.Get<JwtOptions>()!;

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    NameClaimType = JwtRegisteredClaimNames.Sub,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

        return services;
    }

    /// <summary>
    /// Registers the outbox pattern processor and its configuration options.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services.AddOptions<OutboxOptions>()
            .BindConfiguration(OutboxOptions.SectionName);
        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}
