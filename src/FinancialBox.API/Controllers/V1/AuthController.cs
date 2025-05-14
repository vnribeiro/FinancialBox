using Asp.Versioning;
using FinancialBox.API.Contracts;
using FinancialBox.API.Dtos.Auth;
using FinancialBox.API.Extensions;
using FinancialBox.Application.Features.Commands.Auth.Login;
using FinancialBox.Application.Features.Commands.Auth.Register;
using FinancialBox.BuildingBlocks.Mediator;
using Microsoft.AspNetCore.Mvc;


namespace FinancialBox.API.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;

    public AuthController(ILogger<AuthController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LoginUserResponse>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LoginUserResponse>>> Login([FromBody] LoginUserDto dto, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(dto.Name);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            onSuccess: data => Ok(ApiResponse<LoginUserResponse>.FromSuccess(data)),
            onFailure: errors => BadRequest(ApiResponse<LoginUserResponse>.FromErrors(errors)));
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<RegisterUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<RegisterUserResponse>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<RegisterUserResponse>>> Register([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(dto.Name);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            onSuccess: data => Ok(ApiResponse<RegisterUserResponse>.FromSuccess(data)),
            onFailure: errors => BadRequest(ApiResponse<RegisterUserResponse>.FromErrors(errors)));
    }
}