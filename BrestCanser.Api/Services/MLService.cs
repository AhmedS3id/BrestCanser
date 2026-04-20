using BrestCanser.Api.Clients.MLModel;
using BrestCanser.Api.Clients.MLModel.Contracts.MLModel;
using Refit;

namespace BrestCanser.Api.Services;

public class MLService : IMLService
{
	private readonly IMLModelClient _mLModelClient;
	private readonly ApplicationDbContext _context;
	private readonly IImageService _imageService;
	private readonly IServiceProvider _serviceProvider;


	public MLService(
		IMLModelClient mLModelClient,
		ApplicationDbContext context,
		IImageService imageService,
		IServiceProvider serviceProvider)
	{
		_mLModelClient = mLModelClient;
		_context = context;
		_imageService = imageService;
		_serviceProvider = serviceProvider;
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

		// fire & forget result notification	
		//_ = _notificationService.SendPredictionNotificationAsync(userId, history, cancellationToken);

		_ = Task.Run(async () =>
		{
			using var scope = _serviceProvider.CreateScope();

			try
			{
				var notificationService = scope.ServiceProvider
					.GetRequiredService<INotificationService>();

				await notificationService.SendPredictionNotificationAsync(
					userId, history, CancellationToken.None);
			}
			catch (Exception ex)
			{
				var logger = scope.ServiceProvider.GetRequiredService<ILogger<MLService>>();
				logger.LogError(ex, "Failed to send prediction notification for user {UserId}", userId);
			}
		}, cancellationToken);

		return Result.Success(response);
	}

}