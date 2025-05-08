using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.API.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register()
    {
        return Ok();
    }
}