using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinancialBox.Presentation.Swagger;

/// <summary>
/// Configures Swagger generation options to support API versioning in the Financial Box API documentation.
/// </summary>
/// <remarks>This class is typically used to register multiple Swagger documents, one for each API version,
/// enabling versioned API documentation in Swagger UI. It is intended for use with ASP.NET Core dependency injection
/// and SwaggerGen.</remarks>
/// <param name="provider">The provider that supplies descriptions for each available API version. Cannot be null.</param>
internal sealed class ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"Financial Box API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "API for managing financial goals, transactions, and reporting.",
                Contact = new OpenApiContact() { Name = "Vinicius Ribeiro", Email = "contact.vnribeiro@gmail.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
            });
        }
    }
}
