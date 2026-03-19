using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.DomainEvents;
using FinancialBox.Application.Features.Auth;
using FinancialBox.Application.Features.Auth.Commands.Login;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatorImpl = FinancialBox.Application.Mediator.Mediator;

namespace FinancialBox.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(LoginCommand).Assembly;

        services.AddScoped<IMediator, MediatorImpl>();

        services.AddValidatorsFromAssembly(assembly);

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(c => c.AssignableTo(typeof(IPipelineBehavior<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddOptions<AuthOptions>()
            .BindConfiguration(AuthOptions.SectionName);

        return services;
    }
}
