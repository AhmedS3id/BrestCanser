using BrestCanser.Api.Clients.MLModel.Contracts.MLModel;

namespace BrestCanser.Api.Services;

public interface IMLService
{
	Task<Result<PredictionResponse>> PredictAsync(PredictRequest request, string userId, CancellationToken cancellationToken);
}