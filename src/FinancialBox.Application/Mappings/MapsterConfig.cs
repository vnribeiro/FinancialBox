using FinancialBox.Application.Features.Commands.Auth.Login;
using FinancialBox.Application.Features.Commands.Auth.Register;
using Mapster;

namespace FinancialBox.Application.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<LoginUserCommand, LoginUserResponse>.NewConfig();
        TypeAdapterConfig<RegisterUserCommand, RegisterUserResponse>.NewConfig();
    }
}
