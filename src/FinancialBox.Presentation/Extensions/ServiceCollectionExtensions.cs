using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FinancialBox.Application.Contracts.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        // Loads environment configs and user secrets (in dev)
        builder.AddEnvironmentConfiguration();

        // Registers Swagger + JWT support
        services.AddSwaggerConfiguration();

        // Registers default CORS policy
        services.AddCorsConfiguration();

        // Registers authentication/authorization
        services.AddAuthenticationConfiguration(builder);

        // Enables URL-based API versioning
        services.AddApiVersioningConfiguration();

        // Configure routing to use lowercase URLs for consistency and SEO benefits
        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
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
    /// Configures Swagger with API versioning and JWT authentication.
    /// Generates a Swagger document for each API version and secures endpoints using Bearer tokens.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var apiVersionDescriptionProvider = services
                .BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"Financial Box API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = "API for managing financial goals, transactions, and reporting.",
                    Contact = new OpenApiContact() { Name = "Vin�cius Ribeiro", Email = "viniciuscostaa.ribeiro@outlook.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
                });
            }

            // JWT Authentication for Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter the JWT token like this: Bearer {your token}",
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

        var jwtOptions = jwtSection.Get<JwtOptions>()
            ?? throw new InvalidOperationException("Jwt configuration section is missing or invalid.");

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
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
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    /// <summary>
    /// Configures the environment for the application.
    /// Loads JSON configuration files and user secrets for development environments.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder to configure.</param>
    /// <returns>The updated WebApplicationBuilder.</returns>
    private static WebApplicationBuilder AddEnvironmentConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        if (!builder.Environment.IsDevelopment())
            return builder;

        builder.Configuration.AddUserSecrets<Program>();

        return builder;
    }
}
