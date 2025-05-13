using FinancialBox.Application.Common.Behaviors;
using FinancialBox.Application.Common.Mediator;
using FinancialBox.Application.Features.Auth.Login;
using FinancialBox.Application.Features.Auth.Register;
using FinancialBox.Shared.Contracts.Behaviors;
using FinancialBox.Shared.Contracts.Mediator;
using FinancialBox.Shared.ResultObjects;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialBox.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();

            // Register FluentValidation validators from Application assembly
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            // Register pipeline behaviors
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));

            // Register pipeline handlers
            services.AddScoped<IRequestHandler<LoginUserCommand, Result<LoginUserDto>>, LoginUserCommandHandler>();
            services.AddScoped<IRequestHandler<RegisterUserCommand, Result<RegisterUserDto>>, RegisterUserCommandHandler>();

            return services;
        }
    }
}
