using BrestCanser.Api.Enum;
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

	[HttpGet("with-status")]
	public async Task<IActionResult> GetHistoryWithStatus([FromQuery] PredictionStatus? status)
	{
		var result = await _historyService.GetHistoryWithStatusAsync(User.GetUserId()!, status);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}
}