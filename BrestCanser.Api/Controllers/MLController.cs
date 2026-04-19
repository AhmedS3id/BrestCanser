using BrestCanser.Api.Clients.MLModel;
using BrestCanser.Api.Clients.MLModel.Contracts.MLModel;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace BrestCanser.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Microsoft.AspNetCore.Authorization.Authorize]
public class MLController(IMLModelClient _mLModelClient) : ControllerBase
{
	[HttpPost("")]
	public async Task<IActionResult> UploadFile([FromForm] PredictRequest request)
	{
		var streamPart = new StreamPart(request.File.OpenReadStream(), request.File.FileName, request.File.ContentType);

		var response = await _mLModelClient.PredictAsync(streamPart);

		return Ok(response);
	}
}
