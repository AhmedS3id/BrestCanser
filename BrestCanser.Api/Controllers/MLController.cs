using BrestCanser.Api.Clients.MLModel;
using BrestCanser.Api.Clients.MLModel.Contracts.MLModel;
using BrestCanser.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace BrestCanser.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Microsoft.AspNetCore.Authorization.Authorize]
public class MLController(IMLModelClient _mLModelClient,
	ApplicationDbContext _context,
	IImageService _imageService) : ControllerBase
{
	public IMLModelClient MLModelClient { get; } = _mLModelClient;
	public ApplicationDbContext Context { get; } = _context;
	public IImageService ImageService { get; } = _imageService;

	[HttpPost("")]
	public async Task<IActionResult> UploadFile([FromForm] PredictRequest request, CancellationToken cancellationToken)
	{
		var streamPart = new StreamPart(request.File.OpenReadStream(), request.File.FileName, request.File.ContentType);

		var response = await MLModelClient.PredictAsync(streamPart);


		var history = response.Prediction.Adapt<PredictionHistory>();

		history.UserId = User.GetUserId()!;

		//upload the file to cloudinary
		var uploadResult = await ImageService.UploadAsync(request.File, "ClassificationHistory", cancellationToken);
		history.ImageUrl = uploadResult.ImageUrl;

		Context.PredictionHistories.Add(history);
		await Context.SaveChangesAsync(cancellationToken);

		return Ok(response);
	}
}
