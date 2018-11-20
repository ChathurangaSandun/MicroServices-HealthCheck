using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.HealthChecks;

namespace HealthCheck.Controllers
{
    [Produces("application/json")]
    [Route("HealthCheck")]
    public class HealthCheckController : Controller
    {
        private readonly IHealthCheckService _healthCheckService;

        public HealthCheckController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Can also call RunCheckAsync to run a single Heatlh Check or RunGroupAsync to run a group of Health Checks
          CompositeHealthCheckResult healthCheckResult = await _healthCheckService.CheckHealthAsync();

            bool somethingIsWrong = healthCheckResult.CheckStatus != CheckStatus.Healthy;

            if (somethingIsWrong)
            {
                // healthCheckResult has a .Description property, but that shows the description of all health checks. 
                // Including the successful ones, so let's filter those out
                var failedHealthCheckDescriptions = healthCheckResult.Results.Where(r => r.Value.CheckStatus != CheckStatus.Healthy)
                                                                     .Select(r => r.Value.Description)
                                                                     .ToList();

                // return a 500 with JSON containing the Results of the Health Check
                return new JsonResult(new { Errors = failedHealthCheckDescriptions }) { StatusCode = StatusCodes.Status500InternalServerError };
            }

            return new JsonResult(new { Description = healthCheckResult.Description, Status = healthCheckResult.CheckStatus.ToString() });
        }
    }
}