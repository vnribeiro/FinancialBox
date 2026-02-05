using Asp.Versioning;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Features.Auth.Commands.Login;
using FinancialBox.Application.Features.Auth.Commands.Register;
using FinancialBox.Presentation.Extensions;
using FinancialBox.Presentation.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialBox.Presentation.Controllers.V1;

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
    [ProducesResponseType(typeof(ApiResponse<LoginUserResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<LoginUserResponse>>> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(command, cancellationToken);

        return result.Match(Ok);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<RegisterUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<RegisterUserResponse>>> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(command, cancellationToken);

        return result.Match(response => Created("me", response));
    }

    //[Authorize]
    //[HttpGet("me")]
    //[ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> GetMe()
    //{
    //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    //    var result = await _mediator.Send(new GetUserQuery(Guid.Parse(userId)));

    //    return result.Match(
    //        user => Ok(user),
    //        error => NotFound()
    //    );
    //}
}
