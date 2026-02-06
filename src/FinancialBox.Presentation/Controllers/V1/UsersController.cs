using Asp.Versioning;
using FinancialBox.Application.Contracts.Messaging;
using FinancialBox.Application.Contracts.Services;
using FinancialBox.Presentation.Extensions;
using FinancialBox.Presentation.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinancialBox.Application.Features.Users.Queries.GetMe;

namespace FinancialBox.Presentation.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/[controller]")]
public class UsersController(IMediator mediator, ICurrentUserService currentUserService) : Controller
{
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<GetMeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetMeResponse>>> GetMe()
    {
        var userId = currentUserService.UserId;

        var result = await mediator.SendAsync(new GetMeQuery(Guid.Parse(userId!)));

        return result.Match(Ok);
    }
}
