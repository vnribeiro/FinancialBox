using FinancialBox.Application.Features.Auth.Commands.Login;
using FinancialBox.Application.Features.Auth.Commands.Register;
using FinancialBox.Domain.Entities;
using Mapster;

namespace FinancialBox.Application.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<LoginUserCommand, User>.NewConfig();
        TypeAdapterConfig<User, LoginUserResponse>.NewConfig();

        TypeAdapterConfig<RegisterUserCommand, User>.NewConfig();
        TypeAdapterConfig<User, RegisterUserResponse>.NewConfig();
    }
}
