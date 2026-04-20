using BrestCanser.Api.Clients.MLModel;
using BrestCanser.Api.Clients.MLModel.Contracts.MLModel;
using Refit;

namespace BrestCanser.Api.Services;

public class MLService : IMLService
{
	private readonly IMLModelClient _mLModelClient;
	private readonly ApplicationDbContext _context;
	private readonly IImageService _imageService;

	public MLService(
		IMLModelClient mLModelClient,
		ApplicationDbContext context,
		IImageService imageService)
	{
		_mLModelClient = mLModelClient;
		_context = context;
		_imageService = imageService;
	}

	public async Task<Result<PredictionResponse>> PredictAsync(PredictRequest request, string userId, CancellationToken cancellationToken)
	{

		var streamPart = new StreamPart(request.File.OpenReadStream(), request.File.FileName, request.File.ContentType);

		var response = await _mLModelClient.PredictAsync(streamPart);


		if (response?.Prediction is null)
			return Result.Failure<PredictionResponse>(MLErrors.InvalidPrediction);

		var uploadResult = await _imageService.UploadAsync(request.File, ImageFolders.ClassificationHistory, cancellationToken);

		var history = response.Prediction.Adapt<PredictionHistory>();

		history.UserId = userId;
		history.ImageUrl = uploadResult.ImageUrl;

		_context.PredictionHistories.Add(history);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success(response);
	}
}