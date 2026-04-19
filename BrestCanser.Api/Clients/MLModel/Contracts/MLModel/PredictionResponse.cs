using System.Text.Json.Serialization;

namespace BrestCanser.Api.Clients.MLModel.Contracts.MLModel;

public record PredictionResponse(
	[property: JsonPropertyName("prediction")] PredictionResult Prediction,
	[property: JsonPropertyName("success")] bool Success
);
