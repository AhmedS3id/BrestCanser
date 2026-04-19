using BrestCanser.Api.Clients.MLModel.Contracts.MLModel;
using Refit;

namespace BrestCanser.Api.Clients.MLModel;

public interface IMLModelClient
{
	[Multipart]
	[Post("/predict")]
	Task<PredictionResponse> PredictAsync([AliasAs("file")] StreamPart stream);
}
