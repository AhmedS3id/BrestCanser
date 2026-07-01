using System.Text.Json.Serialization;

namespace BrestCanser.Api.Clients.MLModel.Contracts.MLModel;
public record PredictionResponse(

	[property: JsonPropertyName("label")] string Label,

	[property: JsonPropertyName("confidence")] double Confidence,

	[property: JsonPropertyName("probabilities")]
	ProbabilitiesResponse Probabilities,

	[property: JsonPropertyName("mask")]
	string Mask
);