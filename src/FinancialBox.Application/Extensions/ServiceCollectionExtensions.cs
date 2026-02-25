using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.DomainEvents;
using FinancialBox.Application.Features.Auth.Commands.Login;
using FinancialBox.Application.Options;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatorImpl = FinancialBox.Application.Mediator.Mediator;

namespace FinancialBox.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Get the assembly of the Application layer
            var applicationAssembly = typeof(LoginCommand).Assembly;

            // Register custom Mediator implementation
            services.AddScoped<IMediator, MediatorImpl>();

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

            // Register all IDomainEventHandler<TEvent> implementations from Application
            services.Scan(scan => scan
                .FromAssemblies(applicationAssembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Register options
            services.AddOptions<EmailVerificationOptions>()
                .BindConfiguration(EmailVerificationOptions.SectionName);

            return services;
        }
    }
}
