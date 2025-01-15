using Microsoft.AspNetCore.Mvc;

namespace FinancialBox.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialGoalsController : ControllerBase
    {
        private readonly ILogger<FinancialGoalsController> _logger;

        public FinancialGoalsController(ILogger<FinancialGoalsController> logger)
        {
            _logger = logger;
        }
    }
}
