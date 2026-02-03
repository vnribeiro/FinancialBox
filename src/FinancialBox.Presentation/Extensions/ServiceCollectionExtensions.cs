using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace FinancialBox.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Extension method to add presentation layer services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Central entry point to register all API-related configurations:
        /// - Swagger
        /// - CORS
        /// - API Versioning
        /// - Environment settings
        /// </summary>
        /// <param name="builder">The WebApplicationBuilder instance.</param>
        /// <returns>The updated service collection.</returns>
        public IServiceCollection AddPresentation(WebApplicationBuilder builder)
        {
            // Registers Swagger + JWT support
            services.AddSwaggerConfiguration();

            // Registers default CORS policy
            services.AddCorsConfiguration();

            // Enables URL-based API versioning
            services.AddApiVersioningConfiguration();

            // Loads environment configs and user secrets (in dev)
            builder.AddEnvironmentConfiguration();

            return services;
        }

        /// <summary>
        /// Adds API versioning to the service collection.
        /// Configures the API to use versioning based on URL segments.
        /// Reports available API versions and substitutes the API version in the URL.
        /// </summary>
        /// <returns>The updated service collection.</returns>
        private IServiceCollection AddApiVersioningConfiguration()
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
        private IServiceCollection AddSwaggerConfiguration()
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
                        Contact = new OpenApiContact() { Name = "Vinícius Ribeiro", Email = "viniciuscostaa.ribeiro@outlook.com" },
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
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });

            return services;
        }

        /// <summary>
        /// Adds Cross-Origin Resource Sharing (CORS) configuration to allow requests from specified origins.
        /// </summary>
        /// <returns>The updated service collection.</returns>
        private IServiceCollection AddCorsConfiguration()
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

