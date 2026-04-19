namespace BrestCanser.Api.Clients.MLModel.Contracts.MLModel;

public record PredictRequest(
	IFormFile File
);