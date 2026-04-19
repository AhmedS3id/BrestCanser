using BrestCanser.Api.Contracts.MLModel;
using Refit;

namespace BrestCanser.Api.Clients;

public interface IMLModelClient
{
	[Multipart]
	[Post("/predict")]
	Task<PredictionResponse> PredictAsync([AliasAs("file")] StreamPart stream);
}
