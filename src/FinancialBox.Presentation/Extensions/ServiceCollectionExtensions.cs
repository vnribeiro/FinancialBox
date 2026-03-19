using Asp.Versioning;
using Microsoft.OpenApi;
using FinancialBox.Presentation.Swagger;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinancialBox.Presentation.Extensions;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Central entry point to register all API-related configurations:
    /// - Swagger
    /// - CORS
    /// - API Versioning
    /// - Environment settings
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="builder">The IHostApplicationBuilder instance.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services, IHostApplicationBuilder builder)
    {
        builder
            .AddLoggingConfiguration();

        services
            .AddSwaggerConfiguration()
            .AddApiVersioningConfiguration()
            .AddCorsConfiguration(builder.Configuration, builder.Environment)
            .AddRouting(o => o.LowercaseUrls = true);

        return services;
    }

    /// <summary>
    /// Configures Serilog for logging with console and file sinks.
    /// </summary>
    /// <param name="builder">The IHostApplicationBuilder to configure.</param>
    /// <returns>The updated IHostApplicationBuilder.</returns>
    private static IHostApplicationBuilder AddLoggingConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSerilog(config => config
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30));

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
    /// Adds Cross-Origin Resource Sharing (CORS) configuration to allow requests from specified origins.
    /// In production, origins are restricted to those defined in "Cors:AllowedOrigins".
    /// In development (no origins configured), all origins are allowed.
    /// </summary>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
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
                else if (environment.IsDevelopment())
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                else
                    throw new InvalidOperationException("Cors:AllowedOrigins must be configured outside Development.");
            });
        });

        return services;
    }
}