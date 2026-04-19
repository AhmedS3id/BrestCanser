using System.Text.Json.Serialization;

namespace BrestCanser.Api.Contracts.MLModel;

public record PredictionResult(
	[property: JsonPropertyName("arabic_label")] string ArabicLabel,
	[property: JsonPropertyName("class_id")] int ClassId,
	[property: JsonPropertyName("class_name")] string ClassName,
	[property: JsonPropertyName("confidence")] double Confidence,
	[property: JsonPropertyName("message")] string Message,
	[property: JsonPropertyName("status")] string Status
);