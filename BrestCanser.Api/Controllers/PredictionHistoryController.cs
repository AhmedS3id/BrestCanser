namespace BrestCanser.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting(RateLimiters.GeneralPolicy)]

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

	[HttpGet("statistics")]
	public async Task<IActionResult> GetStatistics()
	{
		var result = await _historyService.GetStatisticsAsync(User.GetUserId()!);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpGet("report")]
	[EnableRateLimiting(RateLimiters.SensitivePolicy)]
	public async Task<IActionResult> GetReport()
	{
		var result = await _historyService.GenerateReportAsync(User.GetUserId()!);

		if (!result.IsSuccess)
			return result.ToProblem();

		return File(result.Value.FileContents, result.Value.ContentType, result.Value.FileName);
	}
}