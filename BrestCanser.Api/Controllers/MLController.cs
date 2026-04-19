using BrestCanser.Api.Clients.MLModel;
using BrestCanser.Api.Clients.MLModel.Contracts.MLModel;
using BrestCanser.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace BrestCanser.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Microsoft.AspNetCore.Authorization.Authorize]
public class MLController(IMLModelClient _mLModelClient,ApplicationDbContext _context) : ControllerBase
{
	[HttpPost("")]
	public async Task<IActionResult> UploadFile([FromForm] PredictRequest request)
	{
		var streamPart = new StreamPart(request.File.OpenReadStream(), request.File.FileName, request.File.ContentType);

		var response = await _mLModelClient.PredictAsync(streamPart);


		var history = response.Prediction.Adapt<PredictionHistory>();
		
		history.UserId = User.GetUserId()!;

		history.ImageUrl = "https://res.cloudinary.com/ahmedragheb/image/upload/v1774656651/female-avatar_vp22bk.png";

		_context.PredictionHistories.Add(history);
		await _context.SaveChangesAsync();

		return Ok(response);
	}
}
