using FinancialBox.Application.Dispatching;
using FinancialBox.Application.Features.Auth.Commands.Login;
using FinancialBox.BuildingBlocks.DomainEvents;
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

            // Register all IDomainEventHandler<TEvent> implementations
            services.Scan(scan => scan
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}
