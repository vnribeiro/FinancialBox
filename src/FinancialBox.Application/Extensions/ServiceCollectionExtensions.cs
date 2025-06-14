using FinancialBox.Application.Features.Auth.Commands.Login;
using FinancialBox.Application.Interceptors.Mediator;
using FinancialBox.Application.Mappings;
using FinancialBox.BuildingBlocks.Behaviors;
using FinancialBox.BuildingBlocks.Mediator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialBox.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Get the assembly of the Application layer
            var applicationAssembly = typeof(LoginUserCommand).Assembly;

            // Register the mappings from mapster
            MapsterConfig.RegisterMappings();

            // Register custom Mediator implementation
            services.AddScoped<IMediator, Mediator>();

            // Register FluentValidation validators from the Application layer
            services.AddValidatorsFromAssembly(applicationAssembly);

            // Register all IRequestHandler<TRequest, TResponse> implementations
            services.Scan(scan => scan
                .FromAssemblies(applicationAssembly)
                .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Automatically register all IPipelineBehavior<TRequest, TResponse> implementations
            services.Scan(scan => scan
                .FromAssemblies(applicationAssembly)
                .AddClasses(x => x.AssignableTo(typeof(IPipelineBehavior<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}
