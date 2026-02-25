using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinancialBox.Infrastructure.Options;
using FinancialBox.Presentation.Swagger;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinancialBox.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Central entry point to register all API-related configurations:
    /// - Swagger
    /// - CORS
    /// - API Versioning
    /// - Environment settings
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Configures Serilog with console and file sinks
        builder.AddLoggingConfiguration();

        // Registers Swagger + JWT support
        services.AddSwaggerConfiguration();

        // Enables URL-based API versioning
        services.AddApiVersioningConfiguration();

        // Registers authentication/authorization
        services.AddAuthenticationConfiguration(builder);

        // Registers default CORS policy
        services.AddCorsConfiguration(builder.Configuration);

        // Configure routing to use lowercase URLs for consistency and SEO benefits
        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        return services;
    }

    /// <summary>
    /// Configures Serilog for logging with console and file sinks.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder to configure.</param>
    /// <returns>The updated WebApplicationBuilder.</returns>
    private static WebApplicationBuilder AddLoggingConfiguration(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }

    /// <summary>
    /// Configures Swagger with API versioning and JWT authentication.
    /// Generates a Swagger document for each API version and secures endpoints using Bearer tokens.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        // Registers a custom IConfigureOptions to generate Swagger docs for each API version
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

        // Adds JWT authentication support to Swagger UI
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Provide a valid JWT access token. The 'Bearer' prefix is added automatically.",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });

        return services;
    }

    /// <summary>
    /// Adds API versioning to the service collection.
    /// Configures the API to use versioning based on URL segments.
    /// Reports available API versions and substitutes the API version in the URL.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1.0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    /// <summary>
    /// Adds JWT authentication configuration to the service collection.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var jwtSection = builder.Configuration.GetRequiredSection(JwtOptions.SectionName);

        services.AddOptions<JwtOptions>()
            .Bind(jwtSection)
            .Validate(o => !string.IsNullOrWhiteSpace(o.Issuer), "Jwt:Issuer is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Audience), "Jwt:Audience is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Key), "Jwt:Key is required.")
            .Validate(o => o.Key.Length >= 32, "Jwt:Key must be at least 256 bits.")
            .Validate(o => o.ExpiresInHours > 0, "Jwt:ExpiresInHours must be greater than zero.")
            .ValidateOnStart();

        var jwtOptions = jwtSection.Get<JwtOptions>()!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
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
    /// Adds Cross-Origin Resource Sharing (CORS) configuration to allow requests from specified origins.
    /// In production, origins are restricted to those defined in "Cors:AllowedOrigins".
    /// In development (no origins configured), all origins are allowed.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy", builder =>
            {
                if (allowedOrigins.Length > 0)
                    builder.WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                else
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            });
        });

        return services;
    }
}
