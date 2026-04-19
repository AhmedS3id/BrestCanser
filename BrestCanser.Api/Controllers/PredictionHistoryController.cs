using BrestCanser.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrestCanser.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PredictionHistoryController : ControllerBase
{
	private readonly IHistoryService _historyService;

	public PredictionHistoryController(IHistoryService historyService)
	{
		_historyService = historyService;
	}

	[HttpGet("")]
	public async Task<IActionResult> GetHistory()
	{
		var result = await _historyService.GetHistoryAsync(User.GetUserId()!);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}
}


//TODO GET /api/prediction-history?page=1
//TODO GET /api/prediction-history? status = malignant
