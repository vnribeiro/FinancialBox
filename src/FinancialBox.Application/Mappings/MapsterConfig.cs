using FinancialBox.Application.Features.Auth.Commands.Login;
using FinancialBox.Application.Features.Auth.Commands.Register;
using FinancialBox.Domain.Users;
using Mapster;

namespace FinancialBox.Application.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<LoginUserCommand, User>.NewConfig()
            .MapToConstructor(true);

        TypeAdapterConfig<User, LoginUserResponse>.NewConfig();

        TypeAdapterConfig<RegisterUserCommand, User>.NewConfig()
            .MapToConstructor(true);

        TypeAdapterConfig<User, RegisterUserResponse>.NewConfig();
    }
}
