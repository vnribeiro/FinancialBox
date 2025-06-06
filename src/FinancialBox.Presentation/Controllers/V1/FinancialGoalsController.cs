using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.Presentation.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{apiVersion:apiVersion}/financial-goals")]
public class FinancialGoalsController : ControllerBase
{
    private readonly ILogger<FinancialGoalsController> _logger;

    public FinancialGoalsController(ILogger<FinancialGoalsController> logger)
    {
        _logger = logger;
    }
}
