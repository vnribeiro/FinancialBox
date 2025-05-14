using System.Security.Cryptography.X509Certificates;
using FinancialBox.Application.Features.Auth.Login;
using FinancialBox.Application.Features.Auth.Register;
using FinancialBox.Application.Interceptors.Behaviors;
using FinancialBox.Application.Interceptors.Mediator;
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

            // Register custom Mediator implementation
            services.AddScoped<IMediator, Mediator>();

            // Register FluentValidation validators from the Application layer
            services.AddValidatorsFromAssembly(applicationAssembly);

            // Automatically register all IRequestHandler<TRequest, TResponse> implementations
            services.Scan(scan => scan
                .FromAssemblies(applicationAssembly)
                .AddClasses(x => x.AssignableTo(typeof(IRequestHandler<,>)))
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
