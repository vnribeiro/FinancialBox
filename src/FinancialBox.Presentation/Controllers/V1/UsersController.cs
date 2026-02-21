using Asp.Versioning;
using FinancialBox.Application.Abstractions.Pipeline;
using FinancialBox.Presentation.Extensions;
using FinancialBox.Presentation.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinancialBox.Application.Features.Users.Queries.GetMe;

namespace FinancialBox.Presentation.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/[controller]")]
public class UsersController(IMediator mediator) : Controller
{
    [Authorize(Roles = "User")]
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<GetMeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetMeResponse>>> GetMe()
    {
        var userId = User.GetUserId();
        if (!Guid.TryParse(userId, out var parsedUserId))
            return Unauthorized();

        var result = await mediator.SendAsync(new GetMeQuery(parsedUserId));

        return result.Match(Ok);
    }
}
