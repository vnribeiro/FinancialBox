using FinancialBox.API.Dtos.Auth;
using FinancialBox.Application.Features.Commands.Auth.Login;
using FinancialBox.Application.Features.Commands.Auth.Register;
using Mapster;

namespace FinancialBox.API.Mappings;

public static class ApiMapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<LoginUserDto, LoginUserCommand>.NewConfig();
        TypeAdapterConfig<RegisterUserDto, RegisterUserCommand>.NewConfig();
    }
}

