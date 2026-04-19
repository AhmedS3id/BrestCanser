using System.Text.Json.Serialization;

namespace BrestCanser.Api.Contracts.MLModel;

public record PredictionResponse(
	[property: JsonPropertyName("prediction")] PredictionResult Prediction,
	[property: JsonPropertyName("success")] bool Success
);
