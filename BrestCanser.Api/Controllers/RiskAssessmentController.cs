using BrestCanser.Api.Abstractions;
using BrestCanser.Api.Contracts.RiskAssessment;
using BrestCanser.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BrestCanser.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RiskAssessmentController(IRiskAssessmentService assessmentService) : Controller
{
    private readonly IRiskAssessmentService _assessmentService = assessmentService;

    [HttpPost("assess")]
    public async Task<IActionResult> Assess(
        [FromBody] RiskAssessmentRequest request,
        CancellationToken ct)
    {
        var result = await _assessmentService.AssessAsync(request, ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}