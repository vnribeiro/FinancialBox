using Asp.Versioning;
using FinancialBox.API.Contracts;
using FinancialBox.API.Extensions;
using FinancialBox.Application.Features.Auth.Login;
using FinancialBox.Application.Features.Auth.Register;
using FinancialBox.Shared.Contracts.Mediator;
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
    [ProducesResponseType(typeof(ApiResponse<LoginUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LoginUserDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LoginUserDto>>> Login([FromBody] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);

        return result.Match(
            onSuccess: data => Ok(ApiResponse<LoginUserDto>.FromSuccess(data)),
            onFailure: errors => BadRequest(ApiResponse<LoginUserDto>.FromErrors(errors)));
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<RegisterUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<RegisterUserDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<RegisterUserDto>>> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);

        return result.Match(
            onSuccess: data => Ok(ApiResponse<RegisterUserDto>.FromSuccess(data)),
            onFailure: errors => BadRequest(ApiResponse<RegisterUserDto>.FromErrors(errors)));
    }
}