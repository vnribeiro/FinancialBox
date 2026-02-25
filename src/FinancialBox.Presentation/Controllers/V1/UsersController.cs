using Asp.Versioning;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Application.Features.Users.Queries.GetMe;
using FinancialBox.Domain.Features.Users.Errors;
using FinancialBox.Presentation.Extensions;
using FinancialBox.Presentation.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Presentation.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "User")]
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<GetMeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetMeResponse>>> GetMe()
    {
        if (!User.TryGetUserIdAsGuid(out var userId))
            return UserErrors.NotIdentified.ToProblem();

        var result = await mediator.SendAsync(new GetMeQuery(userId));

        return result.Match(Ok);
    }
}
