using System.Text.Json.Serialization;

namespace BrestCanser.Api.Clients.MLModel.Contracts.MLModel;

public record PredictionResult(
	[property: JsonPropertyName("diagnosis")] string Diagnosis,
	[property: JsonPropertyName("confidence")] double Confidence,
	[property: JsonPropertyName("message")] string Message,
	[property: JsonPropertyName("status")] string Status
);