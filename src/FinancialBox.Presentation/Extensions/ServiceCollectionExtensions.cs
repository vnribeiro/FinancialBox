using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FinancialBox.API.Mappings;
using Microsoft.OpenApi.Models;

namespace FinancialBox.API.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Central entry point to register all API-related configurations:
    /// - Swagger
    /// - CORS
    /// - API Versioning
    /// - Environment settings
    /// - API Mapster mappings (DTO → Command)
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Registers Swagger + JWT support
        services.AddSwaggerConfiguration();

        // Registers default CORS policy
        services.AddCorsConfiguration();

        // Enables URL-based API versioning
        services.AddApiVersioningConfiguration();

        // Loads environment configs and user secrets (in dev)
        builder.AddEnvironmentConfiguration();

        // Registers Mapster mappings from DTOs to Commands
        ApiMapsterConfig.RegisterMappings();                

        return services;
    }

    /// <summary>
    /// Adds API versioning to the service collection.
    /// Configures the API to use versioning based on URL segments.
    /// Reports available API versions and substitutes the API version in the URL.
    /// </summary>
    /// <param name="service">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection service)
    {
        service.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1.0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return service;
    }

    /// <summary>
    /// Configures Swagger with API versioning and JWT authentication.
    /// Generates a Swagger document for each API version and secures endpoints using Bearer tokens.
    /// </summary>
    /// <param name="service">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddSwaggerConfiguration(this IServiceCollection service)
    {
        service.AddSwaggerGen(c =>
        {
            var apiVersionDescriptionProvider = service
                .BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                c.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"Financial Box API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = "API for managing financial goals, transactions, and reporting.",
                    Contact = new OpenApiContact() { Name = "Vinícius Ribeiro", Email = "viniciuscostaa.ribeiro@outlook.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
                });
            }

            // JWT Authentication for Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter the JWT token like this: Bearer {your token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        return service;
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

        if (!builder.Environment.IsDevelopment()) return builder;

        builder.Configuration.AddUserSecrets<Program>();

        return builder;
    }

    /// <summary>
    /// Adds Cross-Origin Resource Sharing (CORS) configuration to allow requests from specified origins.
    /// </summary>
    /// <param name="service">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddCorsConfiguration(this IServiceCollection service)
    {
        service.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return service;
    }
}

