using BrestCanser.Api.Clients.MLModel.Contracts.MLModel;
using BrestCanser.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BrestCanser.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Microsoft.AspNetCore.Authorization.Authorize]
[EnableRateLimiting(RateLimiters.SensitivePolicy)]

public class MLController(IMLService _mlService) : ControllerBase
{
	[HttpPost("")]
	public async Task<IActionResult> Predict([FromForm] PredictRequest request, CancellationToken cancellationToken)
	{
		var result = await _mlService.PredictAsync(request, User.GetUserId()!, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}
}