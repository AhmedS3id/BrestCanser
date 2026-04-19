using BrestCanser.Api.Clients;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace BrestCanser.Api.Controllers;
[Route("api/[controller]")]
[ApiController]

public class MLController(IMLModelClient _mLModelClient) : ControllerBase
{
	[HttpPost("")]
	public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
	{
		var streamPart = new StreamPart(file.OpenReadStream(), file.FileName, file.ContentType);

		var response = await _mLModelClient.PredictAsync(streamPart);

		return Ok(response);
	}
}
